using System;
using System.Collections.Generic;

namespace LotRQuotes.Models
{
	public class Movie
	{
        public string _id;
        public string name { get; set; }
        public int runtimeInMinutes;
        public double budgetInMillions;
        public double boxOfficeRevenueInMillions;
        public int academyAwardNominations;
        public int academyAwardWins;
        public double rottenTomatoesScore;

    }

    public class MovieResponse
    {
        public List<Movie> docs;
        public int total;
        public int limit;
        public int offset;
        public int page;
        public int pages;
    }
}
