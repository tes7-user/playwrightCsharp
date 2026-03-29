using Microsoft.Playwright;

public enum FieldType
{
    TextEntry,
    RadioButton,
    Button,
    Dropdown,
    Checkbox
}

public class SignUpFormComponent
{
    private readonly IPage _page;

    public SignUpFormComponent(IPage page)
    {
        _page = page;
    }

    public async Task InteractWithField(string locator, FieldType fieldType, string value = "")
    {
        switch (fieldType)
        {
            case FieldType.TextEntry:
                await _page.Locator(locator).FillAsync(value);
                break;

            case FieldType.RadioButton:
                await _page.Locator(locator).CheckAsync();
                break;

            case FieldType.Button:
                await _page.Locator(locator).ClickAsync();
                break;

            case FieldType.Dropdown:
                await _page.Locator(locator).SelectOptionAsync(value);
                break;

            case FieldType.Checkbox:
                if (value.ToLower() == "true")
                    await _page.Locator(locator).CheckAsync();
                else
                    await _page.Locator(locator).UncheckAsync();
                break;
        }
    }
}
