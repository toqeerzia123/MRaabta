using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRaabta.Models
{
    public class SubMenuModel
    {
        public int MenuId { get; set; }
        public int Menu_id { get; set; }
        public int child_menuid { get; set; }
        public string sub_menu_name { get; set; }
        public string sublink { get; set; }
        public int ProfileId { get; set; }
        public string Hyperlink { get; set; }
        public string SubMenuName { get; set; }
    }
}