using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _23DH114270_MyStore.Models
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; } // Danh sách sản phẩm
        public List<Category> Categories { get; set; } // Danh sách danh mục

        // Bạn có thể thêm các thuộc tính khác nếu cần
    }
}