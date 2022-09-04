using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Globalization;
using CsvHelper;
using Newtonsoft.Json;
using Infrastructure.Dtos;
using System.Diagnostics;
using CsvHelper.Configuration;
using System.Configuration;

namespace SalaryCalculatorApp {
    public partial class Form1 : Form {
        List<WorkSheetDto>? workSheets;
        IEnumerable<WorkSalaryDto>? workSalaries;
        public Form1() {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) {
            var workedTime = maskedTextBox1.Text;
            if (workSalaries is null || workSheets is null || maskedTextBox1.Text == "000:00") {
                ShowMessage("Please complete the form!");
                return;
            }

            List<int> finalIds = new List<int>();
            List<FinalDto> finalModel = new List<FinalDto>();

            if (workSalaries.Count() != workSheets.Count()) {
                string message = "تعداد کاربر ها در فایل ها با هم تطابق ندارد. ادامه میدهید؟";

                if (MessageBox.Show(message, "عدم تطابق", MessageBoxButtons.YesNo) == DialogResult.Yes) {

                    finalIds = DoCalculate(out finalModel);
                    List<int> exceptedList = workSheets.Select(s => s.Id).Except(finalIds).ToList();
                    ShowMessage($" شناسه های زیر در فایل csv  موجود هستند اما در فایل json وجود ندارند! {Environment.NewLine}  {string.Join(',', exceptedList.Select(n => n.ToString()).ToArray())}");
                }
                else return;
            }
            else {
                finalIds = DoCalculate(out finalModel);
            }
            WriteToFile(finalModel);

            ShowMessage("فایل با موفقیت ایجاد شد.");
            OpenFilePath();
        }

        private List<int> DoCalculate(out List<FinalDto> finalModel) {
             finalModel = new List<FinalDto>();
            List<int> finalIds = new List<int>();
            foreach (var workSalary in workSalaries) {
                WorkSheetDto? workSheet = workSheets.FirstOrDefault(q => q.Id == workSalary.Id);
                if (workSheet is null) {
                    ShowMessage($"workSheet not found for {workSalary.Id}");
                }
                else {
                    FinalDto model = Calculator(workSheet, workSalary, maskedTextBox1.Text);
                    finalModel.Add(model);
                    finalIds.Add(model.Id);
                }
            }
            return finalIds;
        }


        private void OpenFilePath() {
            string path = GetInitialDirectory("DesCSVFilePath");
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@path) {
                UseShellExecute = true
            };
            p.Start();
        }

        private void ShowMessage(string message) {
            string finalMessage = message;
            MessageBox.Show(finalMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
        }

        private async void WriteToFile(IEnumerable<FinalDto> finalModel) {
            string path = GetInitialDirectory("DesCSVFilePath");
            string fullPath = Path.Combine(path, "CalculatedSalary.csv");

            if (File.Exists(fullPath)) File.Delete(fullPath);

            using (var writer = new StreamWriter(new FileStream(fullPath, FileMode.OpenOrCreate,FileAccess.ReadWrite), Encoding.UTF8))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8 })) {
                await csv.WriteRecordsAsync(finalModel);
            }
        }

        private FinalDto Calculator(WorkSheetDto workSheet, WorkSalaryDto workSalary, string currentMonthWorklyHour) {
            FinalDto finalModel = new FinalDto();
            decimal sumFunctionTime = ConvertFormatedTimeToMinutes(workSheet.FunctionTime) + ConvertFormatedTimeToMinutes(workSheet.LeaveTime);
            finalModel.SumFunctionTime = ConvertMinutesToTime(sumFunctionTime);
            finalModel.Name = workSheet.Name;
            finalModel.Id = workSheet.Id;
            decimal overtime = sumFunctionTime - ConvertFormatedTimeToMinutes(currentMonthWorklyHour);
            finalModel.CurrentMonthWorklyHour = currentMonthWorklyHour;
            decimal approvedOvertime = overtime;
            finalModel.LeavedTime = workSheet.LeaveTime;
            decimal minutelySalary = Decimal.Divide(workSalary.HourlySalary, 60);
            finalModel.HourlySalary = workSalary.HourlySalary;

            if (workSalary.FixedSalary > 0) { //FixedSalary
                finalModel.Overtime = ConvertMinutesToTime(overtime);
                finalModel.ApprovedOvertime = ConvertMinutesToTime(approvedOvertime);
                finalModel.FixedSalary = workSalary.FixedSalary;
                if (overtime > 0) {// اضافه کار مثبت
                    finalModel.Salary = workSalary.FixedSalary + (approvedOvertime * minutelySalary);
                }
                else { //اضافه کار منفی
                    finalModel.Salary = workSalary.FixedSalary / (ConvertFormatedTimeToMinutes(currentMonthWorklyHour)) * sumFunctionTime;
                }
            }
            else {// HourlySalary
                finalModel.Salary = sumFunctionTime * minutelySalary;
            }
            finalModel.FunctionTime = workSheet.FunctionTime;

            finalModel.Salary = Math.Truncate(finalModel.Salary);
            return finalModel;
        }

        /// <summary>
        /// HHH:mm to minutes (double)
        /// </summary>
        /// <returns></returns>
        private decimal ConvertFormatedTimeToMinutes(string time) {
            var splited = time.Split(":");
            return (int.Parse(splited[0]) * 60) + int.Parse(splited[1]);
        }

        private string ConvertMinutesToTime(decimal minutes) {
            var ts = TimeSpan.FromMinutes((double)minutes);
            return $"{(int)ts.TotalHours} : {System.Math.Abs(ts.Minutes)}";
        }

        private void button1_Click(object sender, EventArgs e) {
            openFileDialog1.InitialDirectory = GetInitialDirectory("SourceSCVFilePath");
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                StreamReader streamReader = File.OpenText(openFileDialog1.FileName);
                CsvReader csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
                workSheets = csvReader.GetRecords<WorkSheetDto>().ToList();
                csvReader.Dispose();
            }
        }
        private string GetInitialDirectory(string configName) {
            var path = ConfigurationManager.AppSettings.Get(configName);
            if (Directory.Exists(path)) {
                return path;
            }
            else {
                return @"C:\";
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            openFileDialog2.InitialDirectory = GetInitialDirectory("JsonFilePath");
            if (openFileDialog2.ShowDialog() == DialogResult.OK) {
                StreamReader streamReader = File.OpenText(openFileDialog2.FileName);
                string workSalaryString = streamReader.ReadToEnd();
                streamReader.Close();
                workSalaries = JsonConvert.DeserializeObject<IEnumerable<WorkSalaryDto>>(workSalaryString);
            }
        }
    }
}
