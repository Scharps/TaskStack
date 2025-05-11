using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskStack.Messages;

public class TaskDeletedMessage(int value) : ValueChangedMessage<int>(value);