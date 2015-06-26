using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;

namespace GraphyPCL
{
    // May have to split this view model. It is currently serving as vm for Detail, Add, and Edit page
    public class ContactViewModel
    {
        private const string c_datetimeFormat = "MMM dd yyyy";

        private const string c_createdDateTagName = "Created Date";

        private const string c_createdLocationTagName = "Created Location";

        public Contact Contact { get; set; }

        /// <summary>
        /// A copy of the contact used as the ViewModel.
        /// Lesson: when edit something, bind the view to a copy of the object, in other words the viewmodel is a copy of the model. When the model changes -> viewmodel changes -> view changes; when view changes -> viewmodel changes -> if user push Done then model changes, if user push Back then discard viewmodel
        /// </summary>
        public Contact ContactCopyBasicInfo { get; set; }

        public string Organization { get; set; }

        public IList<PhoneNumber> PhoneNumbers { get; set; }

        public IList<Email> Emails { get; set; }

        public IList<Url> Urls { get; set; }

        public IList<Address> Addresses { get; set; }

        public IList<SpecialDate> SpecialDates { get; set; }

        public IList<InstantMessage> IMs { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<CompleteTag> CompleteTags { get; set; }

        public IList<ContactTagMap> ContactTagMaps { get; set; }

        public IList<RelatedContact> ContactsLinkedFromThisContact { get; set; }

        public IList<RelatedContact> ContactsLinkedToThisContact { get; set; }

        public IList<CompleteRelationship> CompleteRelationships { get; set; }

        public IList<RelationshipType> RelationshipTypes { get; set; }

        private ICommand _selectContactPhotoCommand;

        public ICommand SelectContactPhotoCommand
        {
            get
            {
                return _selectContactPhotoCommand;
            }
        }
            
        // May be refactor to use optional params but not required!!

        /// <summary>
        /// Use when creating new Contact
        /// </summary>
        public ContactViewModel()
        {
            Contact = new Contact();
            ContactCopyBasicInfo = new Contact();
            _selectContactPhotoCommand = new Command(SelectContactPhoto);

            PhoneNumbers = new List<PhoneNumber>();
            Emails = new List<Email>();
            Urls = new List<Url>();
            IMs = new List<InstantMessage>();

            Addresses = new List<Address>();
            SpecialDates = new List<SpecialDate>();

            Tags = DatabaseManager.GetRows<Tag>();
            CompleteTags = new List<CompleteTag>();

            RelationshipTypes = DatabaseManager.GetRows<RelationshipType>();
            CompleteRelationships = new List<CompleteRelationship>();
        }

        /// <summary>
        /// Use when editting an existing Contact
        /// </summary>
        public ContactViewModel(Contact contact)
        {
            _selectContactPhotoCommand = new Command(SelectContactPhoto);

            this.Contact = contact;

            // Copy the basic info for binding, will have to copy back when user pushes Done
            ContactCopyBasicInfo = new Contact();
            CopyBasicInformation(Contact, ContactCopyBasicInfo);

            Organization = contact.Organization ?? " "; // To display the Org as blank instead of null

            PhoneNumbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id);
            Emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id);
            Urls = DatabaseManager.GetRowsRelatedToContact<Url>(contact.Id);
            IMs = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);

            Addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);
            SpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);

            Tags = DatabaseManager.GetRows<Tag>(); // It seems we don't need this line !!
            ContactTagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            CompleteTags = new List<CompleteTag>();
            foreach (var tagMap in ContactTagMaps)
            {
                var tag = DatabaseManager.GetRow<Tag>(tagMap.TagId);
                CompleteTags.Add(new CompleteTag
                    {
                        TagId = tag.Id,
                        ContactTapMapId = tagMap.Id,
                        ContactId = contact.Id,
                        Name = tag.Name,
                        Detail = tagMap.Detail
                    });
            }

            RelationshipTypes = DatabaseManager.GetRows<RelationshipType>();
            CompleteRelationships = new List<CompleteRelationship>();

            // Relationship (From)
            var fromRelationships = DatabaseManager.GetRelationshipsFromContact(contact.Id);
            ContactsLinkedFromThisContact = new List<RelatedContact>();
            foreach (var relationship in fromRelationships)
            {
                var type = DatabaseManager.GetRow<RelationshipType>(relationship.RelationshipTypeId);
                var relatedContact = DatabaseManager.GetRow<Contact>(relationship.ToContactId);
                var relationshipDetail = relationship.Detail;
                ContactsLinkedFromThisContact.Add(new RelatedContact(relatedContact, type.Name, relationshipDetail));

                CompleteRelationships.Add(new CompleteRelationship
                    {
                        RelationshipId = relationship.Id,
                        Detail = relationship.Detail,
                        RelatedContactId = relatedContact.Id,
                        RelatedContactName = relatedContact.FullName,
                        IsToRelatedContact = true, // from this contact To (=>) the related contact
                        RelationshipTypeId = type.Id,
                        RelationshipTypeName = type.Name
                    });
            }

            // Relationship (To)
            // A bit duplicate, refactor later!!
            var toRelationships = DatabaseManager.GetRelationshipsToContact(contact.Id);
            ContactsLinkedToThisContact = new List<RelatedContact>();
            foreach (var relationship in toRelationships)
            {
                var type = DatabaseManager.GetRow<RelationshipType>(relationship.RelationshipTypeId);
                var relatedContact = DatabaseManager.GetRow<Contact>(relationship.FromContactId);
                var relationshipDetail = relationship.Detail;
                ContactsLinkedToThisContact.Add(new RelatedContact(relatedContact, type.Name, relationshipDetail));

                CompleteRelationships.Add(new CompleteRelationship
                    {
                        RelationshipId = relationship.Id,
                        Detail = relationship.Detail,
                        RelatedContactId = relatedContact.Id,
                        RelatedContactName = relatedContact.FullName,
                        IsToRelatedContact = false, // not from this contact To (=>) the related contact
                        RelationshipTypeId = type.Id,
                        RelationshipTypeName = type.Name
                    });
            }
        }

        private async void SelectContactPhoto()
        {
            // MediaPicker for Android has a bug in the XLabs nuget package.
            // Ref: https://forums.xamarin.com/discussion/33476/xlabs-imediapicker-on-android-not-working/p1
            if (Device.OS == TargetPlatform.Android)
            {
                return;
            }

//            var mediaPicker = DependencyService.Get<IMediaPicker>(); // Old implementation
            var mediaPicker = Resolver.Resolve<IMediaPicker>();
            MediaFile mediaFile = null;
            try
            {
                mediaFile = await mediaPicker.SelectPhotoAsync(new XLabs.Platform.Services.Media.CameraMediaStorageOptions
                    {
                        MaxPixelDimension = 1024
                    });
            }
            catch (TaskCanceledException)
            {
                // If TaskCanceledException is thrown: user cancel then do nothing!!
            }

            var randomName = Guid.NewGuid().ToString() + ".jpg";

            // Old method using IPhotoService
//            var imageSource = ImageSource.FromStream(() => mediaFile.Source);
//            await DependencyService.Get<IPhotoService>().SaveImageToDisk(imageSource, randomName);

            DependencyService.Get<IFileAccess>().WriteStream(randomName, mediaFile.Source);

            ContactCopyBasicInfo.ImageName = randomName;
        }

        /// <summary>
        /// Saves the new contact and all information in this View Model to database.
        /// </summary>
        /// <returns>1: Create, 0: Update</returns>
        public int CreateOrUpdateContact()
        {
            // If contact already exists (Editing): Update the database
            if (Contact.Id != Guid.Empty)
            {
                var db = DatabaseManager.DbConnection;

                // Update simple information
                CopyBasicInformation(ContactCopyBasicInfo, Contact);

                // Track user data
                UserDataManager.CalculateAndSaveUserMetrics(Contact, PhoneNumbers, Emails, Urls, IMs, Addresses, SpecialDates, CompleteTags, CompleteRelationships);

                db.Update(Contact);

                // Update list-based information like phone numbers, emails...
                // Maybe need to refactor to reduce code loop!!
                #region Update list-based info 

                var oldPhoneNumbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(Contact.Id);
                foreach (var number in PhoneNumbers)
                {
                    if (!String.IsNullOrWhiteSpace(number.Number))
                    {
                        if (oldPhoneNumbers.SingleOrDefault(x => x.Id.Equals(number.Id)) == null)
                        {
                            number.ContactId = Contact.Id;
                            DatabaseManager.DbConnection.Insert(number);
                        }
                        else
                        {
                            DatabaseManager.DbConnection.Update(number); 
                        }
                    }
                }
                foreach (var number in oldPhoneNumbers)
                {
                    if (PhoneNumbers.SingleOrDefault(x => x.Id.Equals(number.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(number);
                    }
                }

                var oldEmails = DatabaseManager.GetRowsRelatedToContact<Email>(Contact.Id);
                foreach (var email in Emails)
                {
                    if (!String.IsNullOrWhiteSpace(email.Address))
                    {
                        if (oldEmails.SingleOrDefault(x => x.Id.Equals(email.Id)) == null)
                        {
                            email.ContactId = Contact.Id;
                            DatabaseManager.DbConnection.Insert(email);
                        }
                        else
                        {
                            DatabaseManager.DbConnection.Update(email);
                        }
                    }
                }
                foreach (var email in oldEmails)
                {
                    if (Emails.SingleOrDefault(x => x.Id.Equals(email.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(email);
                    }
                }

                var oldUrls = DatabaseManager.GetRowsRelatedToContact<Url>(Contact.Id);
                foreach (var url in Urls)
                {
                    if (!String.IsNullOrWhiteSpace(url.Link))
                    {
                        if (oldUrls.SingleOrDefault(x => x.Id.Equals(url.Id)) == null)
                        {
                            url.ContactId = Contact.Id;
                            DatabaseManager.DbConnection.Insert(url);
                        }
                        else
                        {
                            DatabaseManager.DbConnection.Update(url); 
                        }
                    }
                }
                foreach (var url in oldUrls)
                {
                    if (Urls.SingleOrDefault(x => x.Id.Equals(url.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(url);
                    }
                }

                var oldInstantMessages = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(Contact.Id);
                foreach (var instantMessage in IMs)
                {
                    if (!String.IsNullOrWhiteSpace(instantMessage.Nickname))
                    {
                        if (oldInstantMessages.SingleOrDefault(x => x.Id.Equals(instantMessage.Id)) == null)
                        {
                            instantMessage.ContactId = Contact.Id;
                            DatabaseManager.DbConnection.Insert(instantMessage);
                        }
                        else
                        {
                            DatabaseManager.DbConnection.Update(instantMessage); 
                        }
                    }
                }
                foreach (var instantMessage in oldInstantMessages)
                {
                    if (IMs.SingleOrDefault(x => x.Id.Equals(instantMessage.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(instantMessage);
                    }
                }

                var oldAddresses = DatabaseManager.GetRowsRelatedToContact<Address>(Contact.Id);
                foreach (var address in Addresses)
                {
                    if (!String.IsNullOrWhiteSpace(address.StreetLine1)
                        || !String.IsNullOrWhiteSpace(address.StreetLine2)
                        || !String.IsNullOrWhiteSpace(address.City)
                        || !String.IsNullOrWhiteSpace(address.Province)
                        || !String.IsNullOrWhiteSpace(address.Country)
                        || !String.IsNullOrWhiteSpace(address.PostalCode))
                    {
                        if (oldAddresses.SingleOrDefault(x => x.Id.Equals(address.Id)) == null)
                        {
                            address.ContactId = Contact.Id;
                            DatabaseManager.DbConnection.Insert(address);
                        }
                        else
                        {
                            DatabaseManager.DbConnection.Update(address); 
                        }
                    }
                }
                foreach (var address in oldAddresses)
                {
                    if (Addresses.SingleOrDefault(x => x.Id.Equals(address.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(address);
                    }
                }

                var oldSpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(Contact.Id);
                foreach (var date in SpecialDates)
                {
                    if (oldSpecialDates.SingleOrDefault(x => x.Id.Equals(date.Id)) == null)
                    {
                        date.ContactId = Contact.Id; // Can use date.ContactId == null as the If condition as well!!
                        DatabaseManager.DbConnection.Insert(date);
                    }
                    else
                    {
                        DatabaseManager.DbConnection.Update(date); 
                    }
                }
                foreach (var date in oldSpecialDates)
                {
                    if (SpecialDates.SingleOrDefault(x => x.Id.Equals(date.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(date);
                    }
                }

                #endregion

                // Update Tags (including create new, update, delete)
                var oldContactTagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(Contact.Id);
                foreach (var completeTag in CompleteTags)
                {
                    if (completeTag.ContactTapMapId == Guid.Empty) // Newly added completeTag
                    {
                        InsertCompleteTagToDatabase(completeTag); 
                    }
                    else
                    {
                        // Update
                        var oldContactTagMap = oldContactTagMaps.FirstOrDefault(x => x.Id == completeTag.ContactTapMapId);
                        oldContactTagMap.Detail = completeTag.Detail;
                        if (!String.IsNullOrEmpty(completeTag.NewTagName))
                        {
                            oldContactTagMap.TagId = CreateOrRetrieveTag(completeTag.NewTagName);
                        }
                        else
                        {
                            oldContactTagMap.TagId = completeTag.TagId;
                        }
                        DatabaseManager.DbConnection.Update(oldContactTagMap);
                    }
                }
                foreach (var map in oldContactTagMaps)
                {
                    if (CompleteTags.SingleOrDefault(x => x.ContactTapMapId.Equals(map.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(map);
                    }
                }

                // Update relationships
                var oldRelationships = DatabaseManager.DbConnection.Table<Relationship>().Where(x => (x.FromContactId.Equals(Contact.Id)) || (x.ToContactId.Equals(Contact.Id))).ToList(); // Get all relationships connect to this Contact
                foreach (var completeRelationship in CompleteRelationships)
                {
                    if (completeRelationship.RelationshipId == Guid.Empty) // newly added relationship
                    {
                        InsertRelationshipToDatabase(completeRelationship); 
                    }
                    else
                    {
                        // Update
                        var oldRelationship = oldRelationships.FirstOrDefault(x => x.Id == completeRelationship.RelationshipId); // Should always find 1
                        oldRelationship.Detail = completeRelationship.Detail;
                        if (!String.IsNullOrEmpty(completeRelationship.NewRelationshipName))
                        {
                            oldRelationship.RelationshipTypeId = CreateOrRetrieveRelationshipType(completeRelationship.NewRelationshipName);
                        }
                        else
                        {
                            oldRelationship.RelationshipTypeId = completeRelationship.RelationshipTypeId;
                        }
                        if (completeRelationship.IsToRelatedContact)
                        {
                            oldRelationship.ToContactId = completeRelationship.RelatedContactId;
                            oldRelationship.FromContactId = Contact.Id;
                        }
                        else
                        {
                            oldRelationship.ToContactId = Contact.Id;
                            oldRelationship.FromContactId = completeRelationship.RelatedContactId;
                        }
                        DatabaseManager.DbConnection.Update(oldRelationship);
                    }
                }
                foreach (var relationship in oldRelationships)
                {
                    if (CompleteRelationships.SingleOrDefault(x => x.RelationshipId.Equals(relationship.Id)) == null)
                    {
                        DatabaseManager.DbConnection.Delete(relationship);
                    }
                }
                return 0; 
            }
            else // New Contact
            {
                CreateContactInDatabase();
                return 1;
            }
        }

        public void CreateAutoAddedTags()
        {
            DatabaseManager.DbConnection.Insert(new ContactTagMap
                {
                    Id = Guid.NewGuid(),
                    Detail = DateTime.Now.ToString(c_datetimeFormat),
                    ContactId = Contact.Id,
                    TagId = DatabaseManager.GetRowsByName<Tag>(c_createdDateTagName).First().Id
                });

            if ((GeolocationManager.CountrySubdivision != null) && (GeolocationManager.CountrySubdivision.AdminName != null))
            {
                DatabaseManager.DbConnection.Insert(new ContactTagMap
                    {
                        Id = Guid.NewGuid(),
                        Detail = GeolocationManager.CountrySubdivision.AdminName + ", " + GeolocationManager.CountrySubdivision.CountryName,
                        ContactId = Contact.Id,
                        TagId = DatabaseManager.GetRowsByName<Tag>(c_createdLocationTagName).First().Id
                    });
            }
        }

        private void CreateContactInDatabase()
        {
            var db = DatabaseManager.DbConnection;

            // Insert new contact
            CopyBasicInformation(ContactCopyBasicInfo, Contact);
            Contact.Id = Guid.NewGuid();
            db.Insert(Contact);

            // Insert info of new contact
            DatabaseManager.InsertList(PhoneNumbers, Contact);
            DatabaseManager.InsertList(Emails, Contact);
            DatabaseManager.InsertList(Urls, Contact);
            DatabaseManager.InsertList(IMs, Contact);
            DatabaseManager.InsertList(Addresses, Contact);
            DatabaseManager.InsertList(SpecialDates, Contact);

            // Insert related info to new contact: tags
            foreach (var completeTag in CompleteTags)
            {
                InsertCompleteTagToDatabase(completeTag);
            }

            foreach (var completeRelationship in CompleteRelationships)
            {
                InsertRelationshipToDatabase(completeRelationship);
            }
        }

        private void InsertCompleteTagToDatabase(CompleteTag completeTag)
        {
            Guid tagId;
            if (!String.IsNullOrEmpty(completeTag.NewTagName)) // User wanted to create new tag type
            {
                tagId = CreateOrRetrieveTag(completeTag.NewTagName);
            }
            else // User picked an existing tag. Even if user did not pick anything, completeTag.TagId is always generated from somewhere !! Bug!!
            {
                tagId = completeTag.TagId;
            }
            DatabaseManager.DbConnection.Insert(new ContactTagMap
                {
                    Id = Guid.NewGuid(),
                    Detail = completeTag.Detail,
                    ContactId = Contact.Id,
                    TagId = tagId
                }); 
        }

        /// <summary>
        /// Create a new tag from the info of CompleteTag.NewTagName.
        /// However, if NewTagName is already existed in DB, return the Id of the existing tag.
        /// </summary>
        /// <returns>The Id of the created or retrieved tag</returns>
        /// <param name="completeTag">Complete tag.</param>
        private Guid CreateOrRetrieveTag(string newTagName)
        {
            // Create a new tag in db if the new tag has a distinct name.
            var oldTags = DatabaseManager.GetRows<Tag>();
            var oldTagWithSameName = oldTags.SingleOrDefault(x => x.Name == newTagName);
            if (oldTagWithSameName == null)
            {
                var newTag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = newTagName
                };
                DatabaseManager.DbConnection.Insert(newTag);
                return newTag.Id;
            }
            else
            {
                return oldTagWithSameName.Id;
            } 
        }

        private void InsertRelationshipToDatabase(CompleteRelationship completeRelationship)
        {
            Guid relationshipTypeId;
            if (!String.IsNullOrEmpty(completeRelationship.NewRelationshipName)) // User wanted to create new relationship type
            {
                relationshipTypeId = CreateOrRetrieveRelationshipType(completeRelationship.NewRelationshipName);
            }
            else // User picked an existing relationship.
            {
                relationshipTypeId = completeRelationship.RelationshipTypeId; 
            }
            var newRelationship = new Relationship
            {
                Id = Guid.NewGuid(),
                Detail = completeRelationship.Detail,
                RelationshipTypeId = relationshipTypeId
            }; 
            if (completeRelationship.IsToRelatedContact)
            {
                newRelationship.ToContactId = completeRelationship.RelatedContactId;
                newRelationship.FromContactId = Contact.Id;
            }
            else
            {
                newRelationship.ToContactId = Contact.Id;
                newRelationship.FromContactId = completeRelationship.RelatedContactId;
            }
            DatabaseManager.DbConnection.Insert(newRelationship);
        }

        private Guid CreateOrRetrieveRelationshipType(string newRelationshipName)
        {
            // Create a new relationship type in db if the new relationship type has a distinct name.
            var oldRelationshipTypes = DatabaseManager.GetRows<RelationshipType>();
            var oldRelationshipTypeWithSameName = oldRelationshipTypes.SingleOrDefault(x => x.Name == newRelationshipName);
            if (oldRelationshipTypeWithSameName == null)
            {
                var newRelationshipType = new RelationshipType
                {
                    Id = Guid.NewGuid(),
                    Name = newRelationshipName 
                };
                DatabaseManager.DbConnection.Insert(newRelationshipType);
                return newRelationshipType.Id;
            }
            else
            {
                return oldRelationshipTypeWithSameName.Id;
            }  
        }

        private void CopyBasicInformation(Contact source, Contact target)
        {
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Organization = source.Organization;
            target.Favorite = source.Favorite;
            target.ImageName = source.ImageName; 
        }
    }
}