using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class PageEntity
    {
        private int _pageindex;//当前页索引
        private int _pagesize;//每页数
        private int _totalcount;//总项数

        public PageEntity(int totalCount = 0, int pageSize = 10, int pageIndex = 1)
        {
            Totalcount = totalCount; // 负数代表不需要返回总数

            Pagesize = pageSize;    // 负数代表返回全部数据，此时_pageindex不起作用

            if (pageIndex > 0)
                Pageindex = pageIndex;
            else
                Pageindex = 1;
        }

        public int Pageindex
        {
            get
            {
                return _pageindex;
            }

            set
            {
                _pageindex = value;
            }
        }

        public int Pagesize
        {
            get
            {
                return _pagesize;
            }

            set
            {
                _pagesize = value;
            }
        }

        public int Totalcount
        {
            get
            {
                return _totalcount;
            }

            set
            {
                _totalcount = value;
            }
        }
    }
}
