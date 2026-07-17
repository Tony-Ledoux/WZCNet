using System;

namespace WZCNet.Exeptions;

public class NotFoundException(string message):Exception(message);
