This component helps you in using Amazons S3 service for storing objects.

Instead of using Amazons API we have simplified the methods used, this also means that not all functions of S3 is supported.


	PutObjectResult response = Client.PutObject ("BucketName", "FileName", bytes);