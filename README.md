# CsQuery.FormFields #

An extension for CsQuery to format a request body from a form element.

This extension simply adds the following extension methods to `IHTMLFormElement`.

`IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, IDomElement submitter)`
- This allows you to get the data set with a specific [submitter](http://www.w3.org/html/wg/drafts/html/master/forms.html#form-submission-algorithm) node (e.g. the button pressed).

`IEnumerable<NameValueType> GetNameValueTypes(this IHTMLFormElement form, bool implicitSubmission)`
- This allows you to get the data set using [implicit submission](http://www.w3.org/html/wg/drafts/html/master/forms.html#implicit-submission) if you pass `true` or no submitter at all if you pass `false`.

## Example ##

Here's a bit of code that downloads Reddit's front page (`CQ.CreateFromUrl`) and interprets the DOM to get the names and values that would be POSTed on log in.

```csharp
// get a CQ context
CQ document = CQ.CreateFromUrl("http://www.reddit.com");

// fill in some form fields
document["#login_login-main input[name=user]"].Val("foo");
document["#login_login-main input[name=passwd]"].Val("bar");

// get the containing form
IHTMLFormElement loginForm = document["#login_login-main"].OfType<IHTMLFormElement>().First();

// get the data representing the form, using implicit submission
IEnumerable<NameValueType> nameValueTypes = loginForm.GetNameValueTypes(true);

// output the results
foreach (var nameValueType in nameValueTypes)
{
    Console.WriteLine("{0} = {1}", nameValueType.Name, nameValueType.Value);
}
```

The output should look something like this:

```
op = login-main
user = foo
passwd = bar
```

## Dependencies ##

I'm currently waiting for my pull request to [CsQuery](https://github.com/jamietre/CsQuery)
be merged into the mainline. In the meantime, there is a dependency on 
[my fork](https://github.com/joelverhagen/CsQuery). This is accomplished with a submodule.

Eventually, I would like my dependency on CsQuery to be via NuGet or even merge these extensions into CsQuery.

## Missing Features ##

### Form Data Set

- `dirname` attributes.
- file uploads (`<input type=file>`).
- correct handling of `<textarea>` values.
- `<keygen>` elements.

### Other

- Generating `HttpContent` from an `IHTMLFormElement`.
- Generating `HttpRequestMessage` from an `IHTMLFormElement` and a URL.


## Testing and Verification ##

The code is well tested, 100% code coverage on the `CsQuery.FormFields` assembly.
