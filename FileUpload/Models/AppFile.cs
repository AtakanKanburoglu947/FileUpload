﻿namespace FileUpload.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }

}
