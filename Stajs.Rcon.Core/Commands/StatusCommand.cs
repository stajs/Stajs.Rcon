﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core.Commands
{
	internal class StatusCommand : ICommand
	{
		public string ToCommandString()
		{
			return "status";
		}
	}
}