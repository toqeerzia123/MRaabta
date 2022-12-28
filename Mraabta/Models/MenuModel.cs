using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class MenuModel
    {
        public int MenuId { get; set; }
        public int Menu_Id { get; set; }
        public string MenuName { get; set; }
        public string Menu_Name { get; set; }
        public string U_NAME { get; set; }
        public int Profile { get; set; }
        public string HyperLink { get; set; }
        public List<SubMenuModel> SubMenus { get; set; }
    }
}