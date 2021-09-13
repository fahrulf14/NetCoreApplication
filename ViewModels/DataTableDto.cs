using System;

namespace SIP.ViewModels
{
    public class DataTableDto
    {
        /// Request sequence number sent by DataTable, same value must be returned in response
        public string sEcho { get; set; }

        /// Text used for filtering
        public string sSearch { get; set; }

        /// Number of records that should be shown in table
        public int iDisplayLength { get; set; }

        /// First record that should be shown(used for paging)
        public int iDisplayStart { get; set; }

        /// Number of columns in table
        public int iColumns { get; set; }

        /// Number of columns that are used in sorting
        public int iSortingCols { get; set; }

        /// Comma separated list of column names
        public string sColumns { get; set; }

        // Get Column Index
        public int iSortCol_0 { get; set; }

        // Get Asc or Dsc Order
        public string sSortDir_0 { get; set; }

        // Status Filter
        public bool? status { get; set; }

        // Type Filter
        public string tipe1 { get; set; }

        public string tipe2 { get; set; }

        public string tipe3 { get; set; }

        public Guid dataId { get; set; }

        public DateTime? min { get; set; }

        public DateTime? max { get; set; }
    }
}
