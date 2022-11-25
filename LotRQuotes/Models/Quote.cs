using System;
using System.Collections.Generic;

namespace LotRQuotes.Models
{
	public class Quote
    {
        public string _id { get; }
        public string dialog { get; set; }
        public string movie;
        public string character;
        public string id;
	}

    public class QuoteResponse
	{
        public List<Quote> docs;
        public int total;
        public int limit;
        public int offset;
        public int page;
        public int pages;
    }
}
