using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Models
{
	//DataContract for Serializing Data - required to serve in JSON format
	[DataContract]
	public class Charts
	{
		public Charts(int label, int y)
		{
			this.Label = label;
			this.Y = y;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "x")]
		public Nullable<int> Label = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<int> Y = null;
	}
}
