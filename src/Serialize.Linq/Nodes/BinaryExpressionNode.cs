﻿using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "B")]
#endif
    #endregion
    public class BinaryExpressionNode : ExpressionNode<BinaryExpression>
    {
        public BinaryExpressionNode(INodeFactory factory, BinaryExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember]
#else
        [DataMember(Name = "C")]
#endif
        #endregion
        public ExpressionNode Conversion { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember]
#else
        [DataMember(Name = "I")]
#endif
        #endregion
        public bool IsLiftedToNull { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember]
#else
        [DataMember(Name = "L")]
#endif
        #endregion
        public ExpressionNode Left { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember]
#else
        [DataMember(Name = "M")]
#endif
        #endregion
        public MethodInfoNode Method { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember]
#else
        [DataMember(Name = "R")]
#endif
        #endregion
        public ExpressionNode Right { get; set; }

        protected override void Initialize(BinaryExpression expression)
        {
            this.Left = this.Factory.Create(expression.Left);
            this.Right = this.Factory.Create(expression.Right);
            this.Conversion = this.Factory.Create(expression.Conversion);
            this.Method = new MethodInfoNode(this.Factory, expression.Method);
            this.IsLiftedToNull = expression.IsLiftedToNull;
        }

        public override Expression ToExpression()
        {
            var conversion = this.Conversion != null ? this.Conversion.ToExpression() as LambdaExpression : null;
            if (this.Method != null && conversion != null)
                return Expression.MakeBinary(
                    this.NodeType,
                    this.Left.ToExpression(), this.Right.ToExpression(),
                    this.IsLiftedToNull,
                    this.Method.ToMemberInfo(),
                    conversion);
            if (this.Method != null)
                return Expression.MakeBinary(
                    this.NodeType,
                    this.Left.ToExpression(), this.Right.ToExpression(),
                    this.IsLiftedToNull,
                    this.Method.ToMemberInfo());
            return Expression.MakeBinary(this.NodeType,
                    this.Left.ToExpression(), this.Right.ToExpression());
        }
    }
}
