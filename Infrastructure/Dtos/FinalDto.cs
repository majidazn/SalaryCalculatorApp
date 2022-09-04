using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos {
    public class FinalDto {
        /// <summary>
        /// شناسه
        /// </summary>
        [Name("شناسه")]
        public int Id { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [Name("نام")]
        public string Name { get; set; }
        /// <summary>
        /// مرخصی
        /// </summary>
        [Name("مرخصی")]
        public string LeavedTime { get; set; }
        /// <summary>
        /// کارکرد
        /// </summary>
        [Name("کارکرد")]
        public string FunctionTime { get; set; }
        /// <summary>
        /// مجموع کارکرد
        /// </summary>
        [Name("مجموع کارکرد")]
        public string SumFunctionTime { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [Name("توضیحات")]
        public string Description { get; set; }
        /// <summary>
        /// ساعت کاری ماه جاری
        /// </summary>
        [Name("ساعت کاری ماه جاری")]
        public string CurrentMonthWorklyHour { get; set; }
        /// <summary>
        /// اضافه کاری
        /// </summary>
        [Name("اضافه کاری")]
        public string Overtime { get; set; }
        /// <summary>
        /// اضافه کاری تایید شده
        /// </summary>
        [Name("اضافه کاری تایید شده")]
        public string ApprovedOvertime { get; set; }
        /// <summary>
        /// مانده اضافه کاری
        /// </summary>
        [Name("مانده اضافه کاری")]
        public int OvertimeBalance { get; set; }
        /// <summary>
        /// قرارداد ثابت
        /// </summary>
        [Name("قرارداد ثابت")]
        public double FixedSalary { get; set; }
        /// <summary>
        /// قرارداد ساعتی
        /// </summary>
        [Name("قرارداد ساعتی")]
        public decimal HourlySalary { get; set; }
        /// <summary>
        /// مبلغ دریافتی
        /// </summary>
        [Name("مبلغ دریافتی")]
        public decimal Salary { get; set; }
    }
}
