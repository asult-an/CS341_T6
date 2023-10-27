# CS341_T6

# Coding Standards

## C# Standards
### File
**Whitespace** - One statement per line (duh!)
**Comments everywhere!**
### Limits
- Columns: 115
- Class Size: 400 lines
- Method size: 80 lines
##### Class Skeleton:
  ```FooClass
	  |
	  + -- Constants
	  + ---- Properties 
	  + ------ Constructor
	  + -------- Methods
  ```

### Syntax
**PascalCase**:
- Directories (`CookNook/Models`)
- Classes (`BizzBuzz.cs`)
- Methods (`GetJarFile()`)
- Public Properties (`public string Username`)

**camelCase**:
- local variables (`int numClicks`)
- private variables (`private int fooId...`)

### XAML Standards
TODO!

### SQL Standards:
**Table names**: lowercase, snake_case (`airports_table`)
**Attribute names**: lowercase, snake_case (`plane_make`)
**Values**: lowercase if internal values 