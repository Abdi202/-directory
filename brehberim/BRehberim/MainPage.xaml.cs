using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BRehberim;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

		if (File.Exists(fileName))
		{
			string data = File.ReadAllText(fileName);
			contacts = JsonSerializer.Deserialize<ObservableCollection<Contact>>(data);
		}

		view.ItemsSource = Contacts;
	}

	public ObservableCollection<Contact> Contacts => contacts;

	ObservableCollection<Contact> contacts = new()
	{
        new Contact(){FirstName="Ali", LastName="Kara", Mail="alikara@mail.com", Phone="0555 555 55 55"},
        new Contact(){FirstName="Ayşe", LastName="Sarı", Mail="aysesari@mail.com", Phone="0555 444 55 55"},
        new Contact(){FirstName="Oya", LastName="Yeşil", Mail="oyayesil@mail.com", Phone="0555 666 55 55"},
        new Contact(){FirstName="Veli", LastName="Beyaz", Mail="velibeyaz@mail.com", Phone="0555 111 55 55"},
        new Contact(){FirstName="Ahmet", LastName="Mavi", Mail="ahmetmavi@mail.com", Phone="0555 222 55 55"},
        new Contact(){FirstName="Mehmet", LastName="Kırmızı", Mail="mehmetkirmizi@mail.com", Phone="0555 333 55 55"},

    };


	private async void AddCommand(object sender, EventArgs e)
	{
		UserPage page = new UserPage(){Title="Kişi Ekle", AddMethod = AddContact};
		await Navigation.PushModalAsync(page);

	}

	public void AddContact(Contact contact)
	{
		contacts.Add(contact);
	}

	private async void EditCommand(object sender, EventArgs e)
	{
		var but = sender as ImageButton;
		if (but != null)
		{
			var id =but.CommandParameter.ToString();
            var user= Contacts.Single(o=>o.ID==id);

            UserPage page = new UserPage(){Title="Kişi Düzenle", User = user };
			await Navigation.PushModalAsync(page);
		}
	}

	private async void DeleteCommand(object sender, EventArgs e)
	{
        var but = sender as ImageButton;
        if (but != null)
        {
            var id =but.CommandParameter.ToString();
            var user= Contacts.Single(o=>o.ID==id);

			var res = await DisplayAlert("Silmeyi onayla", $"'{user.FullName}' silinsin mi?", "EVET", "HAYIR");
			if (res)
				Contacts.Remove(user);
        }
    }

	string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "data.json");
    private void SaveCommand(object sender, EventArgs e)
    {
		string data = JsonSerializer.Serialize(Contacts);
		File.WriteAllText(fileName, data);
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
		var search = sender as SearchBar;
		var list = contacts.Where(o=>o.FullName.ToLower().Contains(search.Text.ToLower()));

		view.ItemsSource = list;
    }
}

public class Contact : INotifyPropertyChanged
{
	public string ID
	{
		get
		{
			if (id == null)
				id = Guid.NewGuid().ToString();

			return id;
		}
		set { id = value; }
	}


	string id, fname, lname, mail, phone, image= "user.png", fullname;
	public string FirstName { get => fname; set { fname = value; NotifyPropertyChanged(); NotifyPropertyChanged("FullName"); } }
	public string LastName { get => lname; set { lname = value; NotifyPropertyChanged(); NotifyPropertyChanged("FullName"); } }

	[JsonIgnore]
	public string FullName => FirstName + " " + LastName;

	public string Mail { get=>mail; set { mail = value; NotifyPropertyChanged(); } }
	public string Phone { get => phone; set { phone = value; NotifyPropertyChanged(); } }

	public string Image { get => image; set { image = value; NotifyPropertyChanged(); } } 

	public event PropertyChangedEventHandler PropertyChanged;

	public void NotifyPropertyChanged([CallerMemberName] string propertyName="")
	{
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}

