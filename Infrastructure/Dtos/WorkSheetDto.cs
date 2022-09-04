using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos {
    public class WorkSheetDto {
        /// <summary>
        /// شناسه
        /// </summary>
       // [Index(0)]
        [Name("شناسه")]
        public int Id { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        //[Index(1)]
        [Name("نام")]
        public string Name { get; set; }
        /// <summary>
        /// حضور
        /// </summary>
       // [Index(2)]
        [Name("حضور")]
        public string Presence { get; set; }
        /// <summary>
        /// خالص
        /// </summary>
       // [Index(3)]
        [Name("خالص")]
        public string PureTime { get; set; }
        /// <summary>
        /// مرخصی
        /// </summary>
       // [Index(4)]
        [Name("مرخصی")]
        public string LeaveTime { get; set; }
        /// <summary>
        /// تاخیر
        /// </summary>
       // [Index(5)]
        [Name("تاخیر")]
        public string DelayTime { get; set; }
        /// <summary>
        /// کارکرد
        /// </summary>
       // [Index(6)]
        [Name("کارکرد")]
        public string FunctionTime { get; set; }
        /// <summary>
        /// تعداد روزهای ناقص
        /// </summary>
       // [Index(7)]
        [Name("تعداد روزهای ناقص")]
        public string IncompleteDaysCount { get; set; }
        /// <summary>
        /// نوع قرارداد
        /// </summary>
        //[Index(8)]
        [Name("نوع قرارداد")]
        public string ContractType { get; set; }
    }
}
