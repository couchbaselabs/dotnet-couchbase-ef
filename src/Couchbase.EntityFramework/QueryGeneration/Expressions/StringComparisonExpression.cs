﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Couchbase.EntityFramework.QueryGeneration.Expressions
{
    /// <summary>
    /// Represents a comparison between two strings
    /// </summary>
    internal class StringComparisonExpression : Expression
    {

        public static readonly ExpressionType[] SupportedOperations =
        {
            ExpressionType.Equal,
            ExpressionType.NotEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual
        };

        public ExpressionType Operation { get; private set; }
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public StringComparison? Comparison { get; set; }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Extension;
            }
        }

        public override Type Type
        {
            get { return typeof(bool); }
        }

        public static StringComparisonExpression Create(ExpressionType operation, Expression left, Expression right, StringComparison? comparison = null)
        {
            if (!SupportedOperations.Contains(operation))
            {
                throw new ArgumentOutOfRangeException("operation");
            }
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            if (left.Type != typeof(string))
            {
                throw new ArgumentException("Expressions must be strings.", "left");
            }
            if (right.Type != typeof(string))
            {
                throw new ArgumentException("Expressions must be strings.", "right");
            }

            return new StringComparisonExpression(operation, left, right, comparison);
        }

        private StringComparisonExpression(ExpressionType operation, Expression left, Expression right, StringComparison? comparison = null)
        {
            Operation = operation;
            Left = left;
            Right = right;
            Comparison = comparison;
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var newLeft = visitor.Visit(Left);
            var newRight = visitor.Visit(Right);

            if ((Left != newLeft) || (Right != newRight))
            {
                return Create(Operation, newLeft, newRight, Comparison);
            }
            else
            {
                return this;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}",
                Left,
                Operation,
                Right);
        }
    }
}
