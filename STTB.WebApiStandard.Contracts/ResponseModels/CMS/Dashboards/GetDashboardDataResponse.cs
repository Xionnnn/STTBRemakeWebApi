using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Dashboards
{
    public class GetDashboardDataResponse
    {
        public int TotalNews { get; set; }
        public int TotalEvent { get; set; }
        public int TotalAcademicProgram { get; set; }
        public int TotalLecturer { get; set; }
        public int TotalAdministrator { get; set; }
        public int TotalVideo { get; set; }
        public int TotalArticle { get; set; }
        public int TotalJournal { get; set; }
        public int TotalMonograf { get; set; }
        public int TotalBuletin { get; set; }

    }
}
