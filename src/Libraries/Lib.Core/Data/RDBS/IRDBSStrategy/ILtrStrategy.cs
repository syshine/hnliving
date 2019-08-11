using Lib.Core.Domain.Ltr;
using System.Data;

namespace Lib.Core
{
    public partial interface IRDBSStrategy
    {
        #region Qxc
        DataTable GetQxcData();
        int AddQxcNumb(QxcEntity entity);
        #endregion

        #region Pl5
        DataTable GetPl5Data();
        int AddPl5Numb(Pl5Entity entity);
        #endregion
    }
}
