using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System.IO;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        static string ONNX_MODEL_PATH = Directory.GetCurrentDirectory() + "/SentimentAnalysisModel.onnx";
        private readonly hotelsprojectContext _context;

        public ReviewsController(hotelsprojectContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet("~/api/Reviews/Hotel")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByHotelId(int hotelid)
        {
            return await _context.Reviews.Where(a=>a.Hotelid == hotelid && a.Isdeleted != true).ToListAsync();
        }
        // GET: api/Reviews
        [HttpGet("~/api/Reviews/Hotel/Happy")]
        public async Task<ActionResult<IEnumerable<Review>>> GetHappyReviewsByHotelId(int hotelid)
        {
            return await _context.Reviews.Where(a => a.Hotelid == hotelid && a.Isdeleted != true && a.Ishappy=="happy").ToListAsync();
        }
        // GET: api/Reviews
        [HttpGet("~/api/Reviews/Hotel/NotHappy")]
        public async Task<ActionResult<IEnumerable<Review>>> GetNotHappyReviewsByHotelId(int hotelid)
        {
            return await _context.Reviews.Where(a => a.Hotelid == hotelid && a.Isdeleted != true && a.Ishappy == "not happy").ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutReview(int id, Review review)
        //{
        //    if (id != review.Reviewid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(review).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ReviewExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult CreateReview(Review review)
        {
            MLContext mlContext = new MLContext();

            var onnxPredictionPipeline = GetPredictionPipeline(mlContext);
            var onnxPredictionEngine = mlContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>(onnxPredictionPipeline);
            var testInput = new OnnxInput
            {
                reviewText = review.Review1
            };
            var prediction = onnxPredictionEngine.Predict(testInput);

            //Console.WriteLine($"review: {prediction.output_label.First()}");

            review.Ishappy = prediction.output_label.First();
            review.Isdeleted = false;
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return Ok(review);

        }
        //public async Task<ActionResult<Review>> PostReview(Review review)
        //{
        //    MLContext mlContext = new MLContext();

        //     var onnxPredictionPipeline = GetPredictionPipeline(mlContext);
        //    var onnxPredictionEngine = mlContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>(onnxPredictionPipeline);
        //    var testInput = new OnnxInput
        //    {
        //        reviewText = review.Review1
        //    };
        //    var prediction = onnxPredictionEngine.Predict(testInput);

        //    Console.WriteLine($"review: {prediction.output_label.First()}");

        //    _context.Reviews.Add(review);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetReview", new { id = review.Reviewid }, review);
        //}

        // DELETE: api/Reviews/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReview(int id)
        //{
        //    var review = await _context.Reviews.FindAsync(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Reviews.Remove(review);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        [HttpPut("~/api/Users/delete/{id}")]
        public async Task<IActionResult> DelReview(int id)
        {
            var review = _context.Reviews.Where(a => a.Reviewid == id).FirstOrDefault();

            review.Isdeleted = true;

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Reviewid == id);
        }

        #region ONNX
        public class OnnxInput
        {
            [ColumnName("input")]
            public string reviewText { get; set; }
        }
        public class OnnxOutput
        {
            [ColumnName("output_label")]
            public string[] output_label { get; set; }
        }
        static ITransformer GetPredictionPipeline(MLContext mlContext)
        {
            var inputColumns = new string[] { "input" };

            var outputColumns = new string[] { "output_label" };
            var onnxPredictionPipeline =
            mlContext
                .Transforms
                .ApplyOnnxModel(
                    outputColumnNames: outputColumns,
                    inputColumnNames: inputColumns,
                    ONNX_MODEL_PATH);
            var emptyDv = mlContext.Data.LoadFromEnumerable(new OnnxInput[] { });

            return onnxPredictionPipeline.Fit(emptyDv);
        }
        #endregion

    }
}
