# .Net Wrapper for Clarizen API v2.0

Shortcuts:
* Clarizen API: https://api.clarizen.com/V2.0/services/
* Clarizen's API documentation: http://usermanual.clarizen.com/rest-api-guide
* Nuget package: https://www.nuget.org/packages/Ekin.Clarizen/

Table of Contents:
- [.Net Wrapper for Clarizen API v2.0](#net-wrapper-for-clarizen-api-v20)
  * [Getting Started](#getting-started)
    + [1. Install Ekin.Clarizen Nuget package](#1-install-ekinclarizen-nuget-package)
    + [2. Test login](#2-test-login)
  * [Code Structure](#code-structure)
    + [If you are familiar with the Clarizen API structure...](#if-you-are-familiar-with-the-clarizen-api-structure)
    + [API helper class](#api-helper-class)
- [Code Samples](#code-samples)
  * [Authentication Examples](#authentication-examples)
    + [Login](#login)
    + [GetSessionInfo](#getsessioninfo)
  * [CRUD & Data Examples](#crud---data-examples)
    + [CreateAndRetrieve](#createandretrieve)
    + [GetTemplateDescriptions](#gettemplatedescriptions)
    + [CreateFromTemplate](#createfromtemplate)
    + [ChangeState](#changestate)
    + [CreateDiscussion](#creatediscussion)
  * [Query Examples](#query-examples)
    + [CZQL](#czql)
    + [Count](#count)
    + [EntityQuery](#entityquery)
    + [AggregateQuery](#aggregatequery)
    + [GroupsQuery](#groupsquery)
    + [NewsFeedQuery](#newsfeedquery)
    + [EntityFeedQuery](#entityfeedquery)
    + [RepliesQuery](#repliesquery)
    + [GetCalendarInfo](#getcalendarinfo)
    + [Search](#search)
  * [Bulk Example](#bulk-example)
  * [Utils Examples](#utils-examples)
  * [SendEmail](#sendemail)
  * [Metadata Examples](#metadata-examples)
    + [DescribeMetadata](#describemetadata)
    + [GetAllEntities](#getallentities)
    + [GetAllEntityRelations](#getallentityrelations)



## Getting Started

### 1. Install Ekin.Clarizen Nuget package

You can install the Ekin.Clarizen Nuget package in your project using the package manager in Visual Studio. The below example uses Visual Studio 2015 Community Edition, which is free of charge.

### 2. Test login

Next, you should test if you can use the API to log in to Clarizen. Replace the username and password below with your Clarizen username/password. 

```csharp
using System;
using Ekin.Clarizen;

namespace ClarizenSamples.Authentication
{
    class Login
    {
        static Login()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            Console.WriteLine("Login successful");
            Console.WriteLine("Server location: {0}", ClarizenAPI.serverLocation);
            Console.WriteLine("Session Id: {0}", ClarizenAPI.sessionId);

            // Your API code goes here...

            if (ClarizenAPI.Logout())
                Console.WriteLine("Logout successful. {0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

When you run the above code, you should see something like this:

If you don't see this, check your username/password.

Note that you can open a free 30-day trial account at https://www.clarizen.com/free-trial.

## Code Structure

### If you are familiar with the Clarizen API structure...

Clarizen_API_V2_0 follows Clarizen's API structure at https://api.clarizen.com/V2.0/services/. Namespace for the project is Ekin.Clarizen, and under that you can find namespaces for the 7 groups of endpoints:
* Ekin.Clarizen.Authentication
* Ekin.Clarizen.Data
* Ekin.Clarizen.Files
* Ekin.Clarizen.Metadata
* Ekin.Clarizen.Applications
* Ekin.Clarizen.Bulk
* Ekin.Clarizen.Utils

The .Net library has (mostly) matching class names (note the differences in camelCasing) for Clarizen endpoints. For example for the https://api.clarizen.com/V2.0/services/metadata/DescribeEntities endpoint there is a .Net function Ekin.Clarizen.Metadata.describeEntities(), which takes as input Ekin.Clarizen.Metadata.Request.describeEntities and one of the outputs is Ekin.Clarizen.Metadata.Result.describeEntities. 

Two exceptions to the Namespace.endpoint name convention are CRUD operations for Data objects (/data/objects) and Metadata objects, which are (at Ekin.Clarizen.Data) objects_get, objects_put, objects_post and objects_delete, and (at Ekin.Clarizen.Metadata) objects_put, objects_delete.

The only exception to the Namespace.Request convention are Query requests, such as /data/EntityQuery, which are located in Ekin.Clarizen.Data.Queries, e.g. Ekin.Clarizen.Data.Queries.entityQuery instead of Ekin.Clarizen.Data.Queries.Request.entityQuery.

### API helper class

With the .Net wrapper, you don't need to use the above structure. There is a Ekin.Clarizen.API() class that provides session and authentication management as well as bulk query execution. All of the above functions can be accessed through the API class. See the ClarizenAPI.DescribeMetadata() example below (note the Pascal case):

```csharp
using System;
using Ekin.Clarizen;
using Ekin.Clarizen.Metadata;

namespace ClarizenSamples.Metadata
{
    class DescribeMetadata_Entity
    {
        static DescribeMetadata_Entity()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string entityName = "User";  // Could be Customer, Project, User, UserGroup, Task, DiscussionPost, etc.
            describeMetadata metadata = ClarizenAPI.DescribeMetadata(new string[] { entityName }, new string[] { "relations", "fields" });
            if (metadata.IsCalledSuccessfully)
            {
                Console.WriteLine("Fields for the {0} object:", entityName);
                metadata.Data.entityDescriptions[0].SortFields();
                foreach (fieldDescription field in metadata.Data.entityDescriptions[0].fields)
                {
                    Console.WriteLine("\t{0} ({1}) {2}", field.name, field.label, field._type);
                }
            }
            else
                Console.WriteLine(metadata.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

The API class has the following functions:

```csharp
Login(string username, string password)
Logout()
GetSessionInfo()

StartBulkService()
CommitBulkService(bool transactional = false)

GetObject(string id, string[] fields)
GetObject(string id)
CreateObject(string id, object obj)
UpdateObject(string id, object obj)
DeleteObject(string id)
CreateAndRetrieve(object entity, string[] fields)
RetrieveMultiple(Data.Request.retrieveMultiple request)
RetrieveMultiple(string[] fields, string[] ids)

ExecuteQuery(Interfaces.IClarizenQuery query)

Count(IQuery query)
EntityQuery(Data.Queries.entityQuery request)
GetAllEntities(string typeName, string[] fields)
GroupsQuery(Data.Queries.groupsQuery request)
GroupsQuery(string[] fields)
AggregateQuery(Data.Queries.aggregateQuery request)
RelationQuery(Data.Queries.relationQuery request)
NewsFeedQuery(Data.Queries.newsFeedQuery request)
NewsFeedQuery(newsFeedMode mode, string[] fields, string[] feedItemOptions, paging paging)
NewsFeedQuery(newsFeedMode mode, string[] fields)
EntityFeedQuery(Data.Queries.entityFeedQuery request)
EntityFeedQuery(string entityId, string[] fields, string[] feedItemOptions, paging paging)
EntityFeedQuery(string entityId, string[] fields)
RepliesQuery(Data.Queries.repliesQuery request)
RepliesQuery(string postId, string[] fields, string[] feedItemOptions, paging paging)
RepliesQuery(string postId, string[] fields)
ExpenseQuery(Data.Queries.expenseQuery request)
TimesheetQuery(Data.Queries.timesheetQuery request)

Search(Data.Request.search request)
Search(string q)
Search(string q, paging paging)
Search(string q, string typeName, string[] fields)
Search(string q, string typeName, string[] fields, paging paging)
CreateDiscussion(object entity, string[] relatedEntities, string[] notify, string[] topics)
CreateDiscussion(object entity, string[] relatedEntities, string[] notify)
CreateDiscussion(object entity, string[] relatedEntities)
CreateDiscussion(object entity)
CreateDiscussion(Data.Request.createDiscussion request)
Lifecycle(Data.Request.lifecycle request)
Lifecycle(string[] ids, string operation)
ChangeState(string[] ids, string state)
ExecuteCustomAction(Data.Request.executeCustomAction request)
ExecuteCustomAction(string targetId, string customAction, fieldValue[] values)
GetTemplateDescriptions(string typeName)
CreateFromTemplate(object entity, string templateName, string parentId)
GetCalendarInfo(string id)

AppLogin()
SendEmail(recipient[] recipients, string subject, string body, string relatedEntityId, Utils.Request.sendEMail.CZAccessType accessType)

GetApplicationStatus(string applicationId)
InstallApplication(string applicationId, bool autoEnable)

Download(string documentId, bool redirect)
GetUploadUrl()
Upload(Files.Request.upload request)
Upload(string documentId, fileInformation fileInformation, string uploadUrl)
Upload(string documentId, storageType storage, string url, string fileName, string subType, string extendedInfo, string uploadUrl)
UpdateImage(string entityId, string uploadUrl, bool reset)

DescribeMetadata(string[] typeNames, string[] flags)
DescribeMetadata(string[] typeNames)
DescribeMetadata()
ListEntities()
DescribeEntities(string[] typeNames)
DescribeEntityRelations(string[] typeNames)
CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, string action_url, string action_method, string action_headers, string action_body)
CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, action action)
CreateWorkflowRule(Metadata.Request.objects_put request)
DeleteWorkflowRule(string id)
GetSystemSettingsValues(string[] settings)
SetSystemSettingsValues(fieldValue[] settings)
```

# Code Samples

## Authentication Examples

Provides the basic calls to authenticate with the REST API, get session information and access the correct data centre where your organisation is located

### Login

```csharp
using System;
using Clarizen.API.V2_0;

namespace ClarizenSamples.Authentication
{
    class Login
    {
        static Login()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            Console.WriteLine("Login successful");
            Console.WriteLine("Server location: {0}", ClarizenAPI.serverLocation);
            Console.WriteLine("Session Id: {0}", ClarizenAPI.sessionId);

            // Your API code goes here...

            if (ClarizenAPI.Logout())
                Console.WriteLine("Logout successful. {0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### GetSessionInfo

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Authentication;
using ObjectDumper;

namespace ClarizenSamples.Authentication
{
    class GetSessionInfo
    {
        static GetSessionInfo()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            getSessionInfo sessionInfo = ClarizenAPI.GetSessionInfo();
            if (sessionInfo.IsCalledSuccessfully)
                Console.WriteLine(sessionInfo.Data.DumpToString("getSessionInfo"));
            else
                Console.WriteLine(sessionInfo.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

## CRUD & Data Examples

Provides the calls to create, update, retrieve and delete objects in Clarizen

The following example shows how to create, retrieve, update and delete a Task entity.
For this example, let's first create a POCO for Task:

```csharp
namespace Clarizen.POCO
{
    public class Task
    {
        public string id { get; set; }
        public string Name { get; set; }

        public Task() { }

        public Task(string id, string name)
        {
            this.id = id;
            this.Name = name;
        }

        public Task(string name)
        {
            this.Name = name;
        }

    }
}
```

We can then use this object to create a new Task in Clarizen and perform RUD operations on that Task.

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;
using Clarizen.POCO;

namespace ClarizenSamples.Data
{
    class CRUD
    {
        static CRUD()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            // Create a new task
            Task task = new Task("Here is a new task");
            string newTaskId = String.Empty;
            objects_put putObject = ClarizenAPI.CreateObject("/Task", task);
            if (putObject.IsCalledSuccessfully)
            {
                newTaskId = putObject.Data.id;
                Console.WriteLine("New task id: {0}", newTaskId);
            }
            else
                Console.WriteLine(putObject.Error);
            
            if (!String.IsNullOrEmpty(newTaskId))
            {
                // Get task
                objects_get objects = ClarizenAPI.GetObject(newTaskId, new string[] { "Name" });
                if (objects.IsCalledSuccessfully)
                    Console.WriteLine("Task Id: {0}", objects.Data.id);
                else
                    Console.WriteLine(objects.Error);

                // Update task
                Task updateTask = new Task();
                updateTask.Name = "Here is the updated task name";
                objects_post postObject = ClarizenAPI.UpdateObject(newTaskId, updateTask);
                if (postObject.IsCalledSuccessfully)
                    Console.WriteLine("Task updated successfully");
                else
                    Console.WriteLine(postObject.Error);

                // Delete task
                objects_delete deleteObject = ClarizenAPI.DeleteObject(newTaskId);
                if (deleteObject.IsCalledSuccessfully)
                    Console.WriteLine("Task deleted successfully");
                else
                    Console.WriteLine(deleteObject.Error);
            }

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }

    }
}
```

### CreateAndRetrieve

For this method we could use the same Task POCO we created above.

```csharp
namespace Clarizen.POCO
{
    public class Task
    {
        public string id { get; set; }
        public string Name { get; set; }

        public Task() { }

        public Task(string id, string name)
        {
            this.id = id;
            this.Name = name;
        }

    }
}
```

We can then send this POCO to Clarizen to create a Task and immediately retrieve the newly created entity.

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;
using Clarizen.POCO;

namespace ClarizenSamples.Data
{
    class CreateAndRetrieve
    {
        static CreateAndRetrieve()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            Task task = new Task("/Task", "Here is a new task");

            createAndRetrieve op = ClarizenAPI.CreateAndRetrieve(task, new string[] { "Name" });
            if (op.IsCalledSuccessfully)
            {
                Console.WriteLine("\t{0}\t{1}", op.Data.entity.id, op.Data.entity.Name);
            }
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### GetTemplateDescriptions

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Data
{
    class GetTemplateDescriptions
    {
        static GetTemplateDescriptions()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string typeName = "Project";
            getTemplateDescriptions search = ClarizenAPI.GetTemplateDescriptions(typeName);
            if (search.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} templates found for {1}", search.Data.templates.Length, typeName);
                if (search.Data.templates.Length > 0)
                {
                    foreach (string template in search.Data.templates)
                    {
                        Console.WriteLine("\t{0}", template);
                    }
                }
            }
            else
                Console.WriteLine(search.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### CreateFromTemplate

This example uses the Project POCO:

```csharp
namespace Clarizen.POCO
{
    public class Project
    {
        public string id { get; set; }
        public string Name { get; set; }

        public Project(string id, string name)
        {
            this.id = id;
            this.Name = name;
        }

        public Project(string name)
        {
            this.Name = name;
        }
    }
}
```
```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;
using Clarizen.POCO;

namespace ClarizenSamples.Data
{
    class CreateFromTemplate
    {
        static CreateFromTemplate()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            Project project = new Project("/Project", "Ekin's test project");
            string templateName = "Your First Project";
            string parentId = String.Empty;

            createFromTemplate op = ClarizenAPI.CreateFromTemplate(project, templateName, parentId);
            if (op.IsCalledSuccessfully)
                Console.WriteLine("New entity created. Id: {0}", op.Data.id);
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### ChangeState

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Interfaces;

namespace ClarizenSamples.Data
{
    class ChangeState
    {
        static ChangeState()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string newState = "Completed";  // Could be Draft, for example

            // First we will get all tasks with due dates earlier than yesterday that are not Completed
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            IClarizenQuery query = new Query()
                                       .Select("Name,Work,State")
                                       .From("Task")
                                       .Where(String.Format("DueDate<{0} AND State<>\"{1}\"", yesterday, newState));

            Clarizen.API.V2_0.Data.query CZQuery = ClarizenAPI.ExecuteQuery(query);
            if (CZQuery.IsCalledSuccessfully)
            {
                if (CZQuery.Data.entities.Length > 0)
                {
                    // Next, change the state of those tasks to Completed
                    // Note that this does not work for tasks in multiple projects
                    Clarizen.API.V2_0.Data.changeState cs = ClarizenAPI.ChangeState(CZQuery.Data.GetEntityIds(), newState);
                    if (cs.IsCalledSuccessfully)
                    {
                        Console.WriteLine("{0} tasks set to {1}", CZQuery.Data.entities.Length, newState);
                    }
                    else
                        Console.WriteLine(cs.Error);
                }
                else
                    Console.WriteLine("No tasks found with due dates earlier than yesterday");

                if (CZQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones processed here");
            }
            else
                Console.WriteLine(CZQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### CreateDiscussion

This example uses the DiscussionPost POCO:

```csharp
namespace Clarizen.POCO
{
    public class DiscussionPost
    {
        public string id { get; set; }
        public string Body { get; set; }
        public string Container { get; set; }
        public string Via { get; set; }

        public DiscussionPost(string id, string Body, string Container, string Via)
        {
            this.id = id;
            this.Body = Body;
            this.Container = Container;
            this.Via = Via;
        }
    }
}
```
```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.POCO;

namespace ClarizenSamples.Data
{
    class CreateDiscussion
    {
        static CreateDiscussion()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string parentTaskId = "/Task/4batkam0jt4xn0l3cdx4y8ueg4";
            DiscussionPost discussionPost = new DiscussionPost("/DiscussionPost", "This discussion is created by API", parentTaskId, "Integration Tool");
            Clarizen.API.V2_0.Data.createDiscussion op = ClarizenAPI.CreateDiscussion(discussionPost);
            if (op.IsCalledSuccessfully)
                Console.WriteLine("Discussion created successfully");
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

## Query Examples

Provides the calls to query and search for objects in Clarizen

### CZQL

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;
using Clarizen.API.V2_0.Interfaces;

namespace ClarizenSamples.Queries
{
    class CZQL
    {
        static CZQL()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            IClarizenQuery query = new Query()
                      .Select("Name,Work")
                      .From("Task")
                      .Where("StartDate>2016-12-31 AND StartDate<2017-12-31");
            query CZQuery = ClarizenAPI.ExecuteQuery(query);
            if (CZQuery.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} results found", CZQuery.Data.entities.Length);
                foreach (dynamic entity in CZQuery.Data.entities)
                {
                    Console.WriteLine("\t{0}\t{1}", entity.id, entity.Name);
                }
                if (CZQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones shown here");
            }
            else
                Console.WriteLine(CZQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### Count

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data.Queries;
using Clarizen.API.V2_0.Data.Queries.Conditions;
using Clarizen.API.V2_0.Data.Queries.Expressions;

namespace ClarizenSamples.Queries
{
    class Count
    {
        static Count()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            // We will search this term in the DisplayName field of User entities
            string searchTerm = "ekin";

            // Create the search query
            entityQuery query = new entityQuery("User");
            query.where = new compare(new fieldExpression("DisplayName"), Operator.Like, new constantExpression(String.Format("%{0}%", searchTerm)));
            // Here is how to perform the same search using CZQL instead
            //query.where = new cZQLCondition(@"DisplayName LIKE ""%ekin%""");

            // Run the Count method with the query created above
            Clarizen.API.V2_0.Data.countQuery count = ClarizenAPI.Count(query);
            if (count.IsCalledSuccessfully)
                Console.WriteLine("{0} user(s) found with the display name {1}", count.Data.count, searchTerm);
            else
                Console.WriteLine(count.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### EntityQuery

Here is how you could get all Users:

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class EntityQuery_Users
    {
        static EntityQuery_Users()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            entityQuery entityQuery = ClarizenAPI.GetAllEntities("User", new string[] { "FirstName", "LastName", "Email", "UserName" });
            if (entityQuery.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} users found", entityQuery.Data.entities.Length);
                foreach (dynamic user in entityQuery.Data.entities)
                {
                    Console.WriteLine("\t{0}\t{1} {2}, {3} (Username: {4})", user.id, user.FirstName, user.LastName, user.Email, user.UserName);
                }
                if (entityQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones shown here");
            }
            else
                Console.WriteLine(entityQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

And here is how you could get all entities of a type and display their Name property:

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class EntityQuery_ByName
    {
        static EntityQuery_ByName()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string entityName = "Task";  // Could be any entity that has the Name field, e.g. Project
            entityQuery entityQuery = ClarizenAPI.GetAllEntities(entityName, new string[] { "Name" });
            if (entityQuery.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} {1} entities found", entityQuery.Data.entities.Length, entityName);
                foreach (dynamic entity in entityQuery.Data.entities)
                {
                    Console.WriteLine("\t{0}\t{1}", entity.id, entity.Name);
                }
                if (entityQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones shown here");
            }
            else
                Console.WriteLine(entityQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### AggregateQuery

This is the example on https://api.clarizen.com/V2.0/services/data/AggregateQuery.

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data.Queries;

namespace ClarizenSamples.Queries
{
    class AggregateQuery
    {
        static AggregateQuery()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;
            
            aggregateQuery query = 
                new aggregateQuery("Task",
                                   new fieldAggregation[] { new fieldAggregation("Count", "Name", "Cnt") },
                                   new string[] { "State" },
                                   new orderBy[] { new orderBy("Cnt", "Descending") });
            Clarizen.API.V2_0.Data.aggregateQuery aggregateQuery = ClarizenAPI.AggregateQuery(query);
            if (aggregateQuery.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} entities found", aggregateQuery.Data.entities.Length);
                foreach (dynamic entity in aggregateQuery.Data.entities)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}", entity.id, entity.State.id, entity.Cnt);
                }
                if (aggregateQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones shown here");
            }
            else
                Console.WriteLine(aggregateQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### GroupsQuery

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class GroupsQuery
    {
        static GroupsQuery()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            entityQuery entityQuery = ClarizenAPI.GetAllEntities("User", new string[] { "FirstName", "LastName", "Email", "UserName" });
            if (entityQuery.IsCalledSuccessfully)
            {
                Console.WriteLine("{0} users found", entityQuery.Data.entities.Length);
                foreach (dynamic user in entityQuery.Data.entities)
                {
                    Console.WriteLine("\t{0}\t{1} {2}, {3} (Username: {4})", user.id, user.FirstName, user.LastName, user.Email, user.UserName);
                }
                if (entityQuery.Data.paging.hasMore)
                    Console.WriteLine("There are more records than the ones shown here");
            }
            else
                Console.WriteLine(entityQuery.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### NewsFeedQuery

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class NewsFeedQuery
    {
        static NewsFeedQuery()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            newsFeedQuery op = ClarizenAPI.NewsFeedQuery(newsFeedMode.All, new string[] { "Body", "Text", "likesCount" });
            if (op.IsCalledSuccessfully)
            {
                Console.WriteLine("Current user has {0} items in his/her news feed", op.Data.items.Length);
                foreach (postFeedItem item in op.Data.items)
                {
                    //Console.WriteLine(item.message);
                    Console.WriteLine("\t{0}\t{1}\t{2} likes", item.message.id, item.message.Body, item.message.likesCount);
                }
            }
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### EntityFeedQuery

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class EntityFeedQuery
    {
        static EntityFeedQuery()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string taskId = "/Task/4batkam0jt4xn0l3cdx4y8ueg4";
            entityFeedQuery op = ClarizenAPI.EntityFeedQuery(taskId, new string[] { "Body", "Text", "likesCount" });
            if (op.IsCalledSuccessfully)
            {
                int itemCount = op.Data.items.Length;
                Console.WriteLine("This task has {0} items in its social feed:", itemCount);
                if (itemCount > 0)
                {
                    foreach (dynamic discussionPost in op.Data.items)
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2} likes", discussionPost.message.id, discussionPost.message.Body, discussionPost.message.likesCount);
                    }
                }
            }
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### RepliesQuery

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class RepliesQuery
    {
        static RepliesQuery()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string discussionPostId = "/DiscussionPost/msg-4npjkyrj89buagdjkxtr4ja1o";
            repliesQuery op = ClarizenAPI.RepliesQuery(discussionPostId, new string[] { "Body", "Text", "likesCount" });
            if (op.IsCalledSuccessfully)
            {
                int itemCount = op.Data.items.Length;
                Console.WriteLine("This DiscussionPost has {0} replies{1}", itemCount, itemCount > 0 ? ":" : "");
                if (itemCount > 0)
                {
                    foreach (dynamic discussionPost in op.Data.items)
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2} likes", discussionPost.message.id, discussionPost.message.Body, discussionPost.message.likesCount);
                    }
                }
            }
            else
                Console.WriteLine(op.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### GetCalendarInfo

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class GetCalendarInfo
    {
        static GetCalendarInfo()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string userId = "/User/4dxj4hbs5pqvmp2utn4q3ggib3";
            getCalendarInfo search = ClarizenAPI.GetCalendarInfo(userId);
            if (search.IsCalledSuccessfully)
            {
                Clarizen.API.V2_0.Data.Result.getCalendarInfo result = (Clarizen.API.V2_0.Data.Result.getCalendarInfo)search.Data;
                Console.WriteLine("Week Starts On: {0}", result.weekStartsOn);
                Console.WriteLine("Weekday information:");
                foreach (dayInformation info in result.weekDayInformation)
                {
                    Console.WriteLine("\tisWorkingDay: {0}\ttotalWorkingHours: {1:0.##}\tstartHour: {2:0.##}\tendHour: {3:0.##}",
                        info.isWorkingDay, info.totalWorkingHours, info.startHour, info.endHour);
                }
                Console.WriteLine("Default working day:");
                Console.WriteLine("\tisWorkingDay: {0}\ttotalWorkingHours: {1:0.##}\tstartHour: {2:0.##}\tendHour: {3:0.##}",
                    result.defaultWorkingDay.isWorkingDay, result.defaultWorkingDay.totalWorkingHours, result.defaultWorkingDay.startHour, result.defaultWorkingDay.endHour);
            }
            else
                Console.WriteLine(search.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### Search

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Data;

namespace ClarizenSamples.Queries
{
    class Search
    {
        static Search()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string q = "ekin";
            search search = ClarizenAPI.Search(q);
            if (search.IsCalledSuccessfully)
                Console.WriteLine("{0} entities found with {1}", search.Data.entities.Length, q);
            else
                Console.WriteLine(search.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

## Bulk Example

Clarizen enables multiple API calls to be made in a single call. You can use this feature using the .Net wrapper as follows:

```csharp
ClarizenAPI.StartBulkService();
ClarizenAPI.DescribeMetadata(new string[] { "User" }, new string[] { "relations", "fields" });
ClarizenAPI.DescribeEntityRelations(new string[] { "User" });
execute bulkService = ClarizenAPI.CommitBulkService();
```

Here is the full code sample:

```csharp
using System;
using System.Configuration;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Bulk;

namespace ClarizenSamples.Bulk
{
    class BulkExample
    {
        static BulkExample()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            ClarizenAPI.StartBulkService();
            ClarizenAPI.DescribeMetadata(new string[] { "User" }, new string[] { "relations", "fields" });
            ClarizenAPI.DescribeEntityRelations(new string[] { "User" });
            execute bulkService = ClarizenAPI.CommitBulkService();

            if (bulkService.IsCalledSuccessfully)
            {
                foreach (response res in bulkService.Data.responses)
                {
                    if (res.statusCode == 200)
                    {
                        //
                        // Result is cast to its target type so it could be used as follows
                        //
                        switch (res.BodyType)
                        {
                            case "Clarizen.API.V2_0.Metadata.Result.describeMetadata":
                                WriteEntityFields(res.body.entityDescriptions[0]);
                                break;
                            case "Clarizen.API.V2_0.Metadata.Result.describeEntityRelations":
                                WriteEntityRelationsDescription(res.body.entityRelations);
                                break;
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Error {0}: {1}", res.statusCode, ((error)res.body).formatted);
                    }
                }
            }
            else
                Console.WriteLine("Bulk service failed. Error: " + bulkService.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }

        static void WriteEntityFields(entityDescription entity)
        {
            Console.WriteLine("Fields for the {0} entity", entity.typeName);
            entity.SortFields();
            foreach (fieldDescription field in entity.fields)
            {
                Console.WriteLine("\t{0} ({1}) {2}", field.name, field.label, field._type);
            }
        }

        static void WriteEntityRelationsDescription(entityRelationsDescription[] entityRelations)
        {
            foreach (entityRelationsDescription description in entityRelations)
            {
                description.SortRelations();
                Console.WriteLine("Relationships for the {0} object:", description.typeName);
                foreach (relationDescription relation in description.relations)
                {
                    Console.WriteLine("\t{0} ({1}) {2}-{3} {4}", relation.name, relation.label, relation.sourceFieldName, relation.relatedTypeName, relation.linkTypeName);
                }
                Console.WriteLine("");
            }
        }
    }
}
```

## Utils Examples

## SendEmail

Here is how to send an email to an internal Clarizen user:

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Utils;

namespace ClarizenSamples.Utils
{
    class SendEmail
    {
        static SendEmail()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            // Set up the email
            recipient[] recipients = new recipient[] { new recipient(recipient.CZRecipientType.To, "ekin@caglar.com", "/User/4dxj4hbs5pqvmp2utn4q3ggib3") };
            string subject = "Hello world";
            string body = "This is an email test from the API";
            string relatedEntityId = String.Empty;
            Clarizen.API.V2_0.Utils.Request.sendEMail.CZAccessType accessType = Clarizen.API.V2_0.Utils.Request.sendEMail.CZAccessType.Public;

            // Send the email
            sendEMail util = ClarizenAPI.SendEmail(recipients, subject, body, relatedEntityId, accessType);
            if (util.IsCalledSuccessfully)
                Console.WriteLine("Email sent successfully");
            else
                Console.WriteLine("Email could not be sent. Error: " + util.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("Logout successful. {0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

To send en email to an external user, you can initialise the recipients variable without an EntityId - as follows:

```csharp
recipient[] recipients = new recipient[] { new recipient(recipient.CZRecipientType.To, "ekin@woto.com", "") };
```

## Metadata Examples

Provides information about the Clarizen data model including supported entities, entity fields and data types, and relations between entities

### DescribeMetadata

Here is how to get the fields (metadata) of an entity:

```csharp
using System;
using System.Configuration;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Metadata;

namespace ClarizenSamples.Metadata
{
    class DescribeMetadata_Entity
    {
        static DescribeMetadata_Entity()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            string entityName = "User";  // Could be Customer, Project, User, UserGroup, Task, DiscussionPost, etc.
            describeMetadata metadata = ClarizenAPI.DescribeMetadata(new string[] { entityName }, new string[] { "relations", "fields" });
            if (metadata.IsCalledSuccessfully)
            {
                Console.WriteLine("Fields for the {0} object:", entityName);
                metadata.Data.entityDescriptions[0].SortFields();
                foreach (fieldDescription field in metadata.Data.entityDescriptions[0].fields)
                {
                    Console.WriteLine("\t{0} ({1}) {2}", field.name, field.label, field._type);
                }
            }
            else
                Console.WriteLine(metadata.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }

    }
}
```

### GetAllEntities

The following sample uses the ListEntities method to get a list (metadata) of all the entities available to the user:

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Metadata;

namespace ClarizenSamples.Metadata
{
    class GetAllEntities
    {
        static GetAllEntities()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            listEntities metadata = ClarizenAPI.ListEntities();
            if (metadata.IsCalledSuccessfully)
            {
                metadata.Data.SortTypeNames();
                Console.WriteLine("{0} entities found", metadata.Data.typeNames.Length);
                // Note that the following query will be truncated at 2000 characters (RestClient.GET limitation)
                describeEntities entities = ClarizenAPI.DescribeEntities(metadata.Data.typeNames);
                if (entities.IsCalledSuccessfully)
                {
                    foreach (entityDescription description in entities.Data.entityDescriptions)
                    {
                        description.SortFields();
                        Console.WriteLine("Fields for the {0} object:", description.typeName);
                        foreach (fieldDescription field in description.fields)
                        {
                            Console.WriteLine("\t{0} ({1}) {2}", field.name, field.label, field._type);
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine(entities.Error);
            }
            else
                Console.WriteLine(metadata.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }
    }
}
```

### GetAllEntityRelations

The following sample uses the DescribeEntityRelations method to get a list (metadata) of all the relationships for all the entities available to the user:

```csharp
using System;
using Clarizen.API.V2_0;
using Clarizen.API.V2_0.Metadata;

namespace ClarizenSamples.Metadata
{
    class GetAllEntityRelations
    {
        static GetAllEntityRelations()
        {
            API ClarizenAPI = new API();
            if (!ClarizenAPI.Login("username", "password"))
                return;

            listEntities metadata = ClarizenAPI.ListEntities();
            if (metadata.IsCalledSuccessfully)
            {
                metadata.Data.SortTypeNames();
                Console.WriteLine("{0} entities found", metadata.Data.typeNames.Length);
                // Note that the following query will be truncated at 2000 characters (RestClient.GET limitation)
                describeEntityRelations entities = ClarizenAPI.DescribeEntityRelations(metadata.Data.typeNames);
                if (entities.IsCalledSuccessfully)
                    WriteEntityRelationsDescription(entities.Data.entityRelations);
                else
                    Console.WriteLine(entities.Error);
            }
            else
                Console.WriteLine(metadata.Error);

            if (ClarizenAPI.Logout())
                Console.WriteLine("{0} API calls made in this session", ClarizenAPI.TotalAPICallsMadeInCurrentSession);
        }

        static void WriteEntityRelationsDescription(entityRelationsDescription[] entityRelations)
        {
            foreach (entityRelationsDescription description in entityRelations)
            {
                description.SortRelations();
                Console.WriteLine("Relationships for the {0} object:", description.typeName);
                foreach (relationDescription relation in description.relations)
                {
                    Console.WriteLine("\t{0} ({1}) {2}-{3} {4}", relation.name, relation.label, relation.sourceFieldName, relation.relatedTypeName, relation.linkTypeName);
                }
                Console.WriteLine("");
            }
        }
    }
}
```
