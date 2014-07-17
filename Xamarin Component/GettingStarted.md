S3 Storage
======================

Setup:

For this framework you need to have an amazon account, read more at http://docs.aws.amazon.com/AmazonSimpleDB/latest/DeveloperGuide/AboutAWSAccounts.html

Before programstart the S3ClientCore must be initialized with an S3ClientCore and you need to implement a SHA256 HmacService

For example like this:

    ServiceContainer.Register<ISHA256Service> (() => new SHA256Service ());
	ServiceContainer.Register<S3ClientCore> (() => new S3ClientCore (Key.AWSAccessKeyId, Key.AWSSecretKey, Region.EUWest_1));

Look in Sample for SHA256Service example

Then use the Client like so

	PutBucketResult response = await Client.PutBucket (BucketName, new CreateBucketConfiguration (LocationConstraint.EUWest_1));
	
	DeleteBucketResult response = await Client.DeleteBucket (BucketName);
	
	ListAllMyBucketsResult response = await Client.GetBuckets ();
	
	ListBucketResult response = await Client.GetBucket (BucketName);
	
	byte[] message = Encoding.UTF8.GetBytes ("hello dolly");
	PutObjectResult response = await Client.PutObject (BucketName, FileName, message);
	
	GetObjectResult response = await Client.GetObject (BucketName, FileName);
	
	DeleteObjectResult response = await Client.DeleteObject (BucketName, FileName);
	
See more examples and full source code at https://github.com/sadiq81/S3Storage