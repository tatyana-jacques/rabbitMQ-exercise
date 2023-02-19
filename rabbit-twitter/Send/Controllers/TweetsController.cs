using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Send.Models;
using Send.DTO;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Send.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TweetsController : ControllerBase
    {
        private readonly TweetContext _context;
        private ConnectionFactory factory;

        public TweetsController(TweetContext context)
        {
            _context = context;

            this.factory = new ConnectionFactory()
            {
                HostName = "142.93.173.18",
                UserName = "admin",
                Password = "devintwitter"
            };
        }

        [HttpPost]
        public async Task<ActionResult<Tweet>> TweetsPost(TweetDTO tweetDTO)
        {
            if (_context.Tweets == null)
            {
                return Problem("Entity set 'TweetsContext.Posts'  is null.");
            }
            var tweet = new Tweet();
            tweet.Name = tweetDTO.Name;
            tweet.Text = tweetDTO.Text;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "tatyana",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = JsonConvert.SerializeObject(tweet);
                var tweetBytes = Encoding.UTF8.GetBytes(body);
                channel.BasicPublish(exchange: "",
                                routingKey: "tatyana",
                                basicProperties: null,
                                body: tweetBytes);
            }
            return tweet;

        }


        //     _context.Tweets.Add(tweet);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetTweet", new { id = tweet.Id }, tweet);
        // }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets()
        {
            if (_context.Tweets == null)
            {
                return NotFound();
            }
            var tweets = await _context.Tweets.ToListAsync();
            // List<PostDTO> list = new List<PostDTO>();
            // foreach (var x in posts)
            // {
            //     var postDTO = new PostDTO();
            //     postDTO.Name = x.Name;
            //     postDTO.Text = x.Text;
            //     postDTO.DataPostagem = x.DataPostagem.ToString("dd/MM/yyyy HH:mm:ss");
            //     list.Add(postDTO);
            // }
            return tweets;
        }

    }
}