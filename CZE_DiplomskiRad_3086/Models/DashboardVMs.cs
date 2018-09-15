using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZE.Web.Areas.Kandidat.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CZE.Web.Models
{
    public class DashboardVMs
    {
        public class DashboardVM
        {
            public int NePromovisaneOcjeneCount { get; set; }
            public int AktivneGrupeCount { get; set; }
            public int CentriCount { get; set; }
            public ChartVM<decimal> UplateKandidataChartVM { get; set; }
            public ChartVM<int> BrojNovihKandidataChartVM { get; set; }
            public List<OcjenaVMs.OcjenaDetailsVM> OcjeneKandidata { get; set; }
        }
        public class ChartData<TXAxis>
        {
            public ChartData()
            {
                X=new List<string>();
                Y=new List<TXAxis>();
            }
            public List<string> X { get; set; }
            public List<TXAxis> Y { get; set; }
        }
        public class ChartVM<TXAxis>
        {
            public ChartVM()
            {
                Data=new ChartData<TXAxis>();           
            }
            public ChartData<TXAxis> Data { get; set; }
            public List<SelectListItem> GodineDDL { get; set; }
        }
    }
}
