namespace Arduino.Models
{
    public class User
    {
        
        public int UserId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
     
        public string Email { get; set; }
        
        public string Telephone { get; set; }
   
        public string ImagePath { get; set; }
        
        public byte[] ImageArray { get; set; }
        
        public string Password { get; set; }
        
        public string ImageFullPath { get; set; }
        
        public string FullName { get; set; }
        
        public int UserTypeId { get; set; }
    }
}
