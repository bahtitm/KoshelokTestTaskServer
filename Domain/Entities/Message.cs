﻿namespace Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public byte[] Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
