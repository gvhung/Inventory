using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;
using Inventory.Business.Entities;

namespace Inventory.Business.Entities
{
    [DataContract]
	public class Product:EntityBase,IIdentifiableEntity
    {
		[DataMember]
	    public bool Archived { get; set; }

		[DataMember]
	    public double Price { get; set; }

		[DataMember]
	    public string Name { get; set; }

		[DataMember]
	    public int ProductId { get; set; }

		[DataMember]
	    public IList<Category> Categories { get; private set; }

	    public int EntityId
	    {
		    get { return ProductId; }
		    set { ProductId = value; }
	    }
    }
}
