using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharedKernel.Linq
{
    internal interface IExpressionCollection : IEnumerable<Expression>
    {
        void Fill();
    }
}