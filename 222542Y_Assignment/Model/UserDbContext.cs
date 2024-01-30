using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp_Core_Identity.Model
{
	public class UserDbContext : IdentityDbContext<User>
	{
		private readonly IConfiguration _configuration;

        public UserDbContext()
        {
        }

        public UserDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public DbSet<AuditLog> AuditLogs { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string connectionString = _configuration.GetConnectionString("UserConnectionString"); optionsBuilder.UseSqlServer(connectionString);
		}
	}

	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string CreditCard { get; set; }
		public string PhoneNumber { get; set; }
		public string BillingAddress { get; set; }
		public string ShippingAddress { get; set; }
		//to be added photo
	}

	public class AuditLog
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Email { get; set; }

		[Required]
		public string Action { get; set; }

		[Required]
		public DateTime Timestamp { get; set; }
	}
}