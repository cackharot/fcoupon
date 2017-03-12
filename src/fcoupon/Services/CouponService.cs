using System;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using fcoupon;

namespace Services
{
  public class CouponService
  {
    MongoClient _client;
    IMongoDatabase _db;
    IMongoCollection<Coupon> _coupons;

    public CouponService(AppSettings appSettings)
    {
	  _client = new MongoClient(appSettings.Database.Url);
	  _db = _client.GetDatabase(appSettings.Database.Name);
      _coupons = _db.GetCollection<Coupon>("Coupons");
    }

		public IEnumerable<Coupon> Search(string code)
	    {
			var filter = Builders<Coupon>.Filter.Empty;
			if (!string.IsNullOrWhiteSpace(code))
			{
				filter = Builders<Coupon>.Filter.Where(x=>x.Code.Equals(code));
			}
		 	return _coupons.Find(filter).SortByDescending(x=>x.CreatedAt).ToList();
	    }

    public Coupon GetById(string id)
    {
			var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId(id));
			return _coupons.Find(filter).FirstOrDefault();
    }


    public void Update(Coupon coupon)
    {
			var existing = GetById(coupon.Id);
			if (existing == null)
			{
				return;
			}

			existing.Code = coupon.Code;
			existing.CanCombineWithOthers = coupon.CanCombineWithOthers;
			existing.Start = coupon.Start;
			existing.End = coupon.End;
			existing.Type = coupon.Type;
			existing.Status = coupon.Status;
			updateTypeData(coupon, existing);

			var filter = Builders<Coupon>.Filter.Eq("_id", new ObjectId(coupon.Id));
			coupon.UpdateAt = DateTime.Now;
			coupon.UpdatedBy = "admin";
      		_coupons.FindOneAndReplace(filter, coupon);
    }

		void updateTypeData(Coupon coupon, Coupon existing)
		{
			switch (coupon.Type)
			{
				case CouponType.FreeDelivery:
					break;
				case CouponType.MaxAmountThresholdDiscount:
					break;
			}
		}

		public Coupon Create(Coupon coupon)
	    {
			coupon.CreatedAt = DateTime.Now;
   			coupon.CreatedBy = "admin";
	      	_coupons.InsertOne(coupon);
	      	return coupon;
	    }
  }
}
