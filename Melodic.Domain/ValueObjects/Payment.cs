using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Domain.ValueObjects;
public record Payment(string CardNumber, int CVV, DateOnly ExpiryDate);