using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public static class PdfExporter
    {
        public static void ExportRankings(
            string filePath,
            string title,
            IEnumerable<(string PlayerName, int Goals)> goals,
            IEnumerable<(string PlayerName, int YellowCards)> yellowCards,
            IEnumerable<(string PlayerName, int Attendance, string Home, string Away, string Score)> attendance)
        {

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.", nameof(filePath));

            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var doc = new Document();
            doc.Info.Title = title;

            var section = doc.AddSection();

            var heading = section.AddParagraph(title);
            heading.Format.Font.Size = 16;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "8pt";

            AddGoalsTable(section, goals);
            section.AddParagraph().AddLineBreak();

            AddYellowCardsTable(section, yellowCards);
            section.AddParagraph().AddLineBreak();

            AddAttendanceTable(section, attendance);

            var renderer = new PdfDocumentRenderer(unicode: true)
            {
                Document = doc
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filePath);
        }

        private static void AddGoalsTable(Section section, IEnumerable<(string PlayerName, int Goals)> rows)
        {
            section.AddParagraph("Goals").Format.Font.Bold = true;

            var table = section.AddTable();
            table.Borders.Width = 0.5;

            table.AddColumn(Unit.FromCentimeter(12));
            table.AddColumn(Unit.FromCentimeter(3));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Cells[0].AddParagraph("Player");
            header.Cells[1].AddParagraph("Goals");

            foreach (var r in rows ?? Array.Empty<(string, int)>())
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(r.PlayerName ?? "");
                row.Cells[1].AddParagraph(r.Goals.ToString());
            }
        }

        private static void AddYellowCardsTable(Section section, IEnumerable<(string PlayerName, int YellowCards)> rows)
        {
            section.AddParagraph("Yellow cards").Format.Font.Bold = true;

            var table = section.AddTable();
            table.Borders.Width = 0.5;

            table.AddColumn(Unit.FromCentimeter(12));
            table.AddColumn(Unit.FromCentimeter(3));

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Cells[0].AddParagraph("Player");
            header.Cells[1].AddParagraph("Yellow cards");

            foreach (var r in rows ?? Array.Empty<(string, int)>())
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(r.PlayerName ?? "");
                row.Cells[1].AddParagraph(r.YellowCards.ToString());
            }
        }

        private static void AddAttendanceTable(Section section, IEnumerable<(string Location, int Attendance, string Home, string Away, string Score)> rows)
        {
            section.AddParagraph("Attendance").Format.Font.Bold = true;

            var table = section.AddTable();
            table.Borders.Width = 0.5;

            table.AddColumn(Unit.FromCentimeter(5));   // Location
            table.AddColumn(Unit.FromCentimeter(3));   // Attendance
            table.AddColumn(Unit.FromCentimeter(4));   // Match
            table.AddColumn(Unit.FromCentimeter(3));   // Score

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Cells[0].AddParagraph("Location");
            header.Cells[1].AddParagraph("Attendance");
            header.Cells[2].AddParagraph("Match");
            header.Cells[3].AddParagraph("Score");

            foreach (var r in rows ?? Array.Empty<(string, int, string, string, string)>())
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(r.Location ?? "");
                row.Cells[1].AddParagraph(r.Attendance.ToString());
                row.Cells[2].AddParagraph($"{r.Home} - {r.Away}");
                row.Cells[3].AddParagraph(r.Score ?? "");
            }
        }
    }
}
