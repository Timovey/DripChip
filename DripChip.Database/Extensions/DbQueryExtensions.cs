﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DripChip.Database.Extensions
{
    public static class DbQueryExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Expression<Func<TSource, bool>> predicate
        )
        {
            if (condition)
            {
                return source.Where(predicate);
            }

            return source;
        }

    }
    }