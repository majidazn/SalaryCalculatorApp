using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dtos {
    public class WorkSalaryDto {
        public int Id { get; set; }
        /// <summary>
        /// حقوق ثابت
        /// </summary>
        public int FixedSalary { get; set; }
        /// <summary>
        /// حقوق ساعتی
        /// </summary>
        public int HourlySalary { get; set; }

    }
}
