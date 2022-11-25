using System;
using System.Collections.Generic;

namespace LotRQuotes.Models
{
	public class Quote
    {
        public string _id;
        public string dialog;
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
