using System;

using System.Collections.Generic;

using System.Linq;

using System.Net;

using System.Net.Http;

using System.Web.Http;

using Ecommerce_Project.Models;

using Ecommerce_Project.DTO;

namespace Ecommerce_Project.Controllers

{

    public class WishListController : ApiController

    {

        majorprojectEntities db = new majorprojectEntities();

        [HttpGet]

        [Route("api/whishlist/{userId}")]

        public IHttpActionResult GetWhishlistItems(int userId)

        {

            try

            {

                var Items = db.Wishlists

                    .Where(c => c.UserID == userId)

                    .Select(c => new

                    {

                        UserID = c.UserID,

                        ProductID = c.ProductID,

                        WishlistID = c.WishlistID,

                        AddedAt = c.AddedAt

                    }).ToList();

                return Ok(Items);

            }

            catch (Exception ex)

            {

                return InternalServerError(ex);

            }

        }

        [HttpPost]

        [Route("api/whishlist")]

        public IHttpActionResult AddToWhishlist(WhishlistDTO Items)

        {

            try

            {

                var existingItem = db.Wishlists.FirstOrDefault(c =>

                    c.UserID == Items.UserID && c.ProductID == Items.ProductID);

                if (existingItem != null)

                {

                    return Ok(new { message = "Item Already added to Whishlist" });

                }

                else

                {

                    db.Wishlists.Add(new Wishlist

                    {

                        UserID = Items.UserID,

                        ProductID = Items.ProductID,

                        AddedAt = DateTime.Now

                    });

                    db.SaveChanges();

                }

                return Ok(new { message = "Item added to Whishlist." });

            }

            catch (Exception ex)

            {

                return InternalServerError(ex);

            }

        }



        [HttpDelete]

        [Route("api/whishlist/{wid}")]

        public IHttpActionResult RemoveWhishlistsItem(int wid)

        {

            try

            {

                var Item = db.Wishlists.Find(wid);

                if (Item == null)

                    return NotFound();

                db.Wishlists.Remove(Item);

                db.SaveChanges();

                return Ok(new { message = "Item removed from Whishlist." });

            }

            catch (Exception ex)

            {

                return InternalServerError(ex);

            }

        }

    }

}

