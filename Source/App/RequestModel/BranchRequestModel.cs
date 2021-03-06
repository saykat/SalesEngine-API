﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.RequestModel
{
    public class BranchRequestModel : BaseRequestModel<Branch>
    {
        public BranchRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
        {
        }

        public override Expression<Func<Branch, bool>> GetExpression()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                ExpressionObj = x => x.Name.Contains(Keyword);
            }

            return ExpressionObj;
        }
    }
}
