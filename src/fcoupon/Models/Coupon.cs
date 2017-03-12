using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models
{
	public class Coupon
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[Required]
		[MinLength(4)]
		[MaxLength(20)]
		public string Code { get; set; }
		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }
		public bool CanCombineWithOthers { get; set; }

		public DateTime? Start { get; set; }
		public DateTime? End { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[Required]
		public CouponType Type { get; set; }

		public double OrderThreshold { get; set; }
		public double Discount { get; set; }
		public double MaxDiscountAmount { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdateAt { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }

		public Coupon()
		{
			Status = true;
		}
	}

	public enum CouponType
	{
		FreeDelivery,
		MaxAmountThresholdDiscount,
		FlatDiscountWithCap,
		FlatDiscountWithoutCap
	}

	public interface ICouponRule
	{
	}

	public class FreeDeliveryCouponRule : ICouponRule
	{
	}

	public class MaxAmountThresholdDiscountCouponRule : ICouponRule
	{
		
	}

	public class FlatDiscountWithCapCouponRule : ICouponRule
	{
		public FlatDiscountWithCapCouponRule() { }

		public FlatDiscountWithCapCouponRule(double discount, double maxDiscountAmount)
		{
			this.Discount = discount;
			this.MaxDiscountAmount = maxDiscountAmount;
		}

		[Required]
		public double Discount { get; set; }
		[Required]
		public double MaxDiscountAmount { get; set; }
	}

	public class FlatDiscountWithoutCapCouponRule : ICouponRule
	{
		[Required]
		public double Discount { get; set; }
	}
}
