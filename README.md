# Spans in CSharp

This project is a simple example of how to use `Span<T>` in csharp to improve the performance of our applications when manipulating strings.

---

## Strings

Strings in csharp are immutable, which means that once a string is created, it cannot be changed. So, if we want to change a string, we have to create a new string and strings are reference types, which means that they are stored on the heap and it's expensive because depends on the garbage collector to free the memory.

E.g.:

Let's say we have a date string and we want to get the `day`, `month` and `year` as integers.

```csharp
string dateAsString = "30 09 2024";

string day = dateAsString.Substring(0, 2);
string month = dateAsString.Substring(3, 2);
string year = dateAsString.Substring(6);

int dayInt = int.Parse(day);
int monthInt = int.Parse(month);
int yearInt = int.Parse(year);

Console.WriteLine($"Day: {dayInt}, Month: {monthInt}, Year: {yearInt}");
```

When we call the `Substring` method, it will create a new string in the memory heap with the value of the substring.

**Heap:**

| Memory Address | Value        |
| -------------- | ------------ |
| 0x0001         | "30 09 2024" |
| 0x0002         | "30"         |
| 0x0003         | "09"         |
| 0x0004         | "2024"       |

**Stack:**

| Memory Address | Value                     |
| -------------- | ------------------------- |
| 0x0001         | Reference to "30 09 2024" |
| 0x0002         | Reference to "30"         |
| 0x0003         | Reference to "09"         |
| 0x0004         | Reference to "2024"       |

In this example, we have created 3 new strings in the memory heap, which is not efficient, because we are creating new strings every time we call the `Substring` method.

---

## Span

This is where the `Span<T>` comes in, it's a _stack-only_ type that can be used to represent a contiguous region of arbitrary memory, in other words, it's a pointer to a memory address.

E.g.:

```csharp
string dateAsString = "30 09 2024";

ReadOnlySpan<char> span = dateAsString.AsSpan();

ReadOnlySpan<char> day = span.Slice(0, 2);
ReadOnlySpan<char> month = span.Slice(3, 2);
ReadOnlySpan<char> year = span.Slice(6);

int dayInt = int.Parse(day);
int monthInt = int.Parse(month);
int yearInt = int.Parse(year);

Console.WriteLine($"Day: {dayInt}, Month: {monthInt}, Year: {yearInt}");
```

When we call the `AsSpan` method, it will create a new `Span<T>` in the stack with the memory address of the string `dateAsString`.
When we call the `Slice` method, it will create a new `Span<T>` in the stack with the memory address of the string `dateAsString` + the start and end indexes of the slice.

**Heap:**

| Memory Address | Value        |
| -------------- | ------------ |
| 0x01           | "30 09 2024" |
| 0x01+0 (2)     | [30]         |
| 0x01+3 (2)     | [09]         |
| 0x01+6         | [2024]       |

**Stack:**

| Memory Address | Value                                                  |
| -------------- | ------------------------------------------------------ |
| 0x01           | Reference to "30 09 2024"                              |
| 0x01+0         | Reference to "30 09 2024" + Offset (starts at index 0) |
| 2              | Length => 0x01+0 (2) => "30"                           |
| 0x01+3         | Reference to "09 2024" + Offset (starts at index 3)    |
| 2              | Length => 0x01+3 (2) => "09"                           |
| 0x01+6         | Reference to "2024" + Offset (starts at index 6)       |

The `Offset` is the start index of the slice and the `Length` is the end index of the slice, which will give us the value `Hello`.
By using `Span<T>` we can avoid the creation of new strings in the memory heap and improve the performance of our application.
