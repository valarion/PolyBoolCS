// PolyBoolCS is a C# port of the polybooljs library
// (c) Copyright 2016, Sean Connelly (@voidqk), http://syntheti.cc
// MIT License
// Project Home - https://github.com/voidqk/polybooljs

namespace PolyBoolCS
{
	using System;
	using System.Collections.Generic;

	public class StatusLinkedList
	{
		#region Fields

		private StatusNode root = new StatusNode();

		#endregion

		#region Public properties

		public bool isEmpty { get { return root.next == null; } }

		public StatusNode head { get { return root.next; } }

		#endregion

		#region Public functions

		public bool exists( StatusNode node )
		{
			if( node == null || node == root )
				return false;

			return true;
		}

		public Transition findTransition( Func<StatusNode, bool> predicate )
		{
			var prev = root;
			var here = root.next;

			while( here != null )
			{
				if( predicate( here ) )
					break;

				prev = here;
				here = here.next;
			}

			return new Transition()
			{
				before = object.ReferenceEquals( prev, root ) ? null : prev.ev,
				after = here != null ? here.ev : null,
				insert = ( node ) =>
				{
					node.prev = prev;
					node.next = here;
					prev.next = node;

					if( here != null )
					{
						here.prev = node;
					}

					return node;
				}
			};
		}
		
		#endregion
	}

	public class EventLinkedList
	{
		#region Fields

		private EventNode root = new EventNode();

		#endregion

		#region Public properties

		public bool isEmpty { get { return root.next == null; } }

		public EventNode head { get { return root.next; } }

		#endregion

		#region Public functions

		public void insertBefore( EventNode node, Func<EventNode, bool> predicate )
		{
			var last = root;
			var here = (EventNode)root.next;

			while( here != null )
			{
				if( predicate( here ) )
				{
					node.prev = here.prev;
					node.next = here;
					here.prev.next = node;
					here.prev = node;

					return;
				}

				last = here;
				here = (EventNode)here.next;
			}

			last.next = node;
			node.prev = last;
			node.next = null;
		}

		#endregion
	}
}