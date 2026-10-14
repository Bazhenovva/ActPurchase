using ActPurchase.Services.Contracts.DTO;
using ActPurchase.Services.Contracts.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ActPurchase.Services.Services
{
    /// <summary>
    /// Сервис генерации Excel-файлов по актам
    /// </summary>
    public class ActExcelService : IActExcelService
    {
        /// <summary>
        /// Формат ячейки по умолчанию: обычный текст без рамки
        /// </summary>
        private const uint DefaultCellFormat = 0;

        /// <summary>
        /// Формат ячейки заголовка: жирный текст без рамки
        /// </summary>
        private const uint HeaderCellFormat = 1;

        /// <summary>
        /// Формат ячейки данных таблицы: обычный текст с рамкой
        /// </summary>
        private const uint BorderedCellFormat = 2;

        /// <summary>
        /// Формат ячейки шапки таблицы: жирный текст с рамкой
        /// </summary>
        private const uint HeaderBorderedCellFormat = 3;

        /// <summary>
        /// Генерирует Excel-файл по акту
        /// </summary>
        public Task<Stream> GenerateActExcelAsync(ActModel act, CancellationToken cancellationToken)
        {
            if (act == null)
            {
                throw new ArgumentNullException(nameof(act));
            }

            var stream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = CreateWorkbook(document);
                var (worksheetPart, sheetData) = CreateWorksheetWithData(workbookPart);

                AddActInfo(sheetData, act);
                AddCounterpartyInfo(sheetData, act.Seller, "Продавец");
                AddCounterpartyInfo(sheetData, act.Buyer, "Покупатель");
                AddDirectorInfo(sheetData, act.Director);
                AddItems(sheetData, act.Items);

                if (workbookPart.Workbook != null)
                {
                    workbookPart.Workbook.Save();
                }

                worksheetPart.Worksheet?.Save();
            }

            stream.Position = 0;
            return Task.FromResult<Stream>(stream);
        }

        /// <summary>
        /// Создаёт книгу Excel и подключает стили
        /// </summary>
        private WorkbookPart CreateWorkbook(SpreadsheetDocument document)
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = CreateStylesheet();

            if (stylesPart.Stylesheet != null)
            {
                stylesPart.Stylesheet.Save();
            }

            return workbookPart;
        }

        /// <summary>
        /// Создаёт лист Excel и возвращает часть листа с данными
        /// </summary>
        private (WorksheetPart worksheetPart, SheetData sheetData) CreateWorksheetWithData(WorkbookPart workbookPart)
        {
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            var workbook = workbookPart.Workbook;
            if (workbook == null)
            {
                throw new InvalidOperationException("Workbook не инициализирован");
            }

            var sheets = workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Акт"
            };
            sheets.Append(sheet);

            return (worksheetPart, sheetData);
        }

        /// <summary>
        /// Заполняет шапку акта
        /// </summary>
        private void AddActInfo(SheetData sheetData, ActModel act)
        {
            AddTitleRow(sheetData, "Акт");
            AddPropertyRow(sheetData, "Номер", act.Number);
            AddPropertyRow(sheetData, "Город", act.City);
            AddPropertyRow(sheetData, "Дата", act.Date.ToString());
            AddPropertyRow(sheetData, "Тип", act.Type);
            AddPropertyRow(sheetData, "Итоговое количество", act.TotalQuantity.ToString());
            AddPropertyRow(sheetData, "Итоговая сумма", act.TotalSum.ToString());
            AddEmptyRow(sheetData);
        }

        /// <summary>
        /// Заполняет блок контрагента (продавца или покупателя)
        /// </summary>
        private void AddCounterpartyInfo(SheetData sheetData, CounterpartyModel? counterparty, string role)
        {
            AddTitleRow(sheetData, role);

            if (counterparty == null)
            {
                AddPropertyRow(sheetData, "Данные", "не указаны");
                AddEmptyRow(sheetData);
                return;
            }

            AddPropertyRow(sheetData, "Наименование", counterparty.CompanyName);
            AddPropertyRow(sheetData, "ИНН", counterparty.Inn);
            AddPropertyRow(sheetData, "КПП", counterparty.Kpp);
            AddPropertyRow(sheetData, "ОГРН", counterparty.Ogrn);
            AddPropertyRow(sheetData, "Расчётный счёт", counterparty.CheckingAccount);
            AddPropertyRow(sheetData, "Корреспондентский счёт", counterparty.CorrespondentAccount);
            AddPropertyRow(sheetData, "БИК", counterparty.Bik);
            AddPropertyRow(sheetData, "Банк", counterparty.BankName);
            AddPropertyRow(sheetData, "Юридический адрес", counterparty.LegalAddress);
            AddEmptyRow(sheetData);
        }

        /// <summary>
        /// Заполняет блок директора
        /// </summary>
        private void AddDirectorInfo(SheetData sheetData, DirectorModel? director)
        {
            AddTitleRow(sheetData, "Директор");

            if (director == null)
            {
                AddPropertyRow(sheetData, "Данные", "не указаны");
                AddEmptyRow(sheetData);
                return;
            }

            AddPropertyRow(sheetData, "Фамилия", director.LastName);
            AddPropertyRow(sheetData, "Имя", director.FirstName);
            AddPropertyRow(sheetData, "Отчество", director.MiddleName ?? string.Empty);
            AddPropertyRow(sheetData, "Организация", director.CompanyName);
            AddEmptyRow(sheetData);
        }

        /// <summary>
        /// Заполняет таблицу позиций акта
        /// </summary>
        private void AddItems(SheetData sheetData, IReadOnlyCollection<ActItemModel>? items)
        {
            AddTitleRow(sheetData, "Позиции");

            if (items == null || items.Count == 0)
            {
                AddPropertyRow(sheetData, "Данные", "позиции отсутствуют");
                AddEmptyRow(sheetData);
                return;
            }

            AddRow(
                sheetData,
                HeaderBorderedCellFormat,
                "№",
                "Наименование",
                "Количество",
                "Цена",
                "Сумма");

            var number = 1;
            foreach (var item in items)
            {
                AddRow(
                    sheetData,
                    BorderedCellFormat,
                    number.ToString(),
                    item.Name,
                    item.Quantity.ToString(),
                    item.Price.ToString(),
                    item.Sum.ToString());
                number++;
            }
        }

        /// <summary>
        /// Добавляет строку с названием и значением
        /// </summary>
        private void AddPropertyRow(SheetData sheetData, string propertyName, string value)
        {
            AddRow(sheetData, DefaultCellFormat, propertyName, value);
        }

        /// <summary>
        /// Добавляет строку-заголовок
        /// </summary>
        private void AddTitleRow(SheetData sheetData, string title)
        {
            AddRow(sheetData, HeaderCellFormat, title);
        }

        /// <summary>
        /// Добавляет пустую строку
        /// </summary>
        private void AddEmptyRow(SheetData sheetData)
        {
            sheetData.AppendChild(new Row());
        }

        /// <summary>
        /// Добавляет строку с ячейками
        /// </summary>
        private void AddRow(SheetData sheetData, uint styleIndex, params string[] values)
        {
            var row = new Row();
            foreach (var value in values)
            {
                var cell = new Cell
                {
                    StyleIndex = styleIndex,
                    DataType = CellValues.String,
                    CellValue = new CellValue(value ?? string.Empty)
                };
                row.AppendChild(cell);
            }
            sheetData.AppendChild(row);
        }

        /// <summary>
        /// Создаёт стили Excel: обычный, жирный, с рамкой, жирный с рамкой
        /// </summary>
        private Stylesheet CreateStylesheet()
        {
            var fonts = new Fonts(
                new Font(
                    new FontName
                    {
                        Val = "Calibri"
                    },
                    new FontSize { Val = 11 }),
                new Font(
                    new Bold(),
                    new FontName
                    {
                        Val = "Calibri"
                    },
                    new FontSize
                    {
                        Val = 11
                    }));

            var fills = new Fills(
                new Fill(new PatternFill
                {
                    PatternType = PatternValues.None
                }),
                new Fill(new PatternFill
                {
                    PatternType = PatternValues.Gray125
                }));

            var borders = new Borders(
                new Border(),
                new Border(
                    new LeftBorder
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new RightBorder
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new TopBorder
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new BottomBorder
                    {
                        Style = BorderStyleValues.Thin
                    }));

            var cellFormats = new CellFormats(
                new CellFormat
                {
                    FontId = 0, FillId = 0, BorderId = 0
                },
                new CellFormat
                {
                    FontId = 1, FillId = 0, BorderId = 0
                },
                new CellFormat
                {
                    FontId = 0, FillId = 0, BorderId = 1
                },
                new CellFormat
                {
                    FontId = 1, FillId = 0, BorderId = 1
                });

            return new Stylesheet(fonts, fills, borders, cellFormats);
        }
    }
}
