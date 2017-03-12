using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace fcoupon.Controllers
{
	[Route("api/[controller]")]
	public class CouponController : Controller
	{
		CouponService service;

		public CouponController(CouponService service)
		{
			this.service = service;
		}

		[HttpGet]
		public IActionResult Search([FromQuery] string code)
		{
			return new ObjectResult(this.service.Search(code));
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			if (string.IsNullOrWhiteSpace(id) || id == "-1")
			{
				return NoContent();
			}
			return new ObjectResult(this.service.GetById(id));
		}

		[HttpPost]
		public IActionResult Create([FromBody] Coupon coupon)
		{
			var reader = new StreamReader(Request.Body);
			Console.WriteLine("Request body");
			Console.WriteLine(reader.ReadToEnd());
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(coupon));
			if (TryUpdateModelAsync(coupon).Result)
			{
				Console.WriteLine("update model success");
			}
			else
			{
				Console.WriteLine(string.Join("\n", ModelState.Select(x => x.Key + ":" + x.Value.Errors.First().ErrorMessage)));
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status="error", errorCode=ErrorCodes.InvalidCouponData, message = "Invalid coupon data posted"});
			}

			service.Create(coupon);
			return new ObjectResult(new { status = "success", data = coupon, location = "/api/coupon/" + coupon.Id });
		}

		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] Coupon coupon)
		{
			var reader = new StreamReader(Request.Body);
			Console.WriteLine("Request body");
			Console.WriteLine(reader.ReadToEnd());
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(coupon));
			if (coupon != null)
			{
				Console.WriteLine("update model success");
			}
			else
			{
				Console.WriteLine(string.Join("\n", ModelState.Select(x => x.Key + ":" + x.Value.ValidationState.ToString())));
			}
			if (!ModelState.IsValid || coupon == null)
			{
				return BadRequest(new { status = "error", errorCode = ErrorCodes.InvalidCouponData, message = "Invalid coupon data posted" });
			}

			coupon.Id = id;
			service.Update(coupon);
			return new ObjectResult(new { status = "success" });
		}
	}
}
