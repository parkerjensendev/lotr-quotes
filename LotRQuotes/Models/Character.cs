using System;
using System.Collections.Generic;

namespace LotRQuotes.Models
{
	public class Character
	{
		public string _id;
		public string height;
		public string race;
        public string gender;
        public string birth;
        public string spouse;
		public string death;
        public string realm;
		public string hair;
		public string name { get; set; }
		public string wikiUrl;
	}

	public class CharacterResponse
	{
		public List<Character> docs;
		public int total;
		public int limit;
		public int offset;
		public int page;
	}
}
