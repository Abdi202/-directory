
namespace BRehberim;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
        
        
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if(User!=null)
        {
            txtName.Text = User.FirstName;
            txtSurname.Text = User.LastName;
            txtMail.Text = User.Mail;
            txtTel.Text = User.Phone;
        }

    }
    public bool Result { get; set; }=false;
    public Contact User { get; set; }
    public Action<Contact> AddMethod { get; internal set; }

    private void btnOK_Clicked(object sender, EventArgs e)
    {
        if (User == null)
        {
            User = new Contact()
            {
                FirstName = txtName.Text,
                LastName = txtSurname.Text,
                Phone = txtTel.Text,
                Mail = txtMail.Text,
            };
        }
        else
        {
            User.FirstName = txtName.Text;
            User.LastName = txtSurname.Text;
            User.Phone = txtTel.Text;
            User.Mail = txtMail.Text;
        }

        if(AddMethod!=null)
            AddMethod(User);

        Result = true;

        Navigation.PopModalAsync();
    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Result=false;
        Navigation.PopModalAsync();

    }
}