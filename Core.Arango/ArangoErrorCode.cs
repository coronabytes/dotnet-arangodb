namespace Core.Arango
{
    public enum ArangoErrorCode
    {
        #region General errors
        /// <summary>No error has occurred.</summary>
        ErrorNoError = 0,

        /// <summary>Will be raised when a general error occurred.</summary>
        ErrorFailed = 1,

        /// <summary>Will be raised when operating system error occurred.</summary>
        ErrorSysError = 2,

        /// <summary>Will be raised when there is a memory shortage.</summary>
        ErrorOutOfMemory = 3,

        /// <summary>Will be raised when an internal error occurred.</summary>
        ErrorInternal = 4,

        /// <summary>Will be raised when an illegal representation of a number was given.</summary>
        ErrorIllegalNumber = 5,

        /// <summary>Will be raised when a numeric overflow occurred.</summary>
        ErrorNumericOverflow = 6,

        /// <summary>Will be raised when an unknown option was supplied by the user.</summary>
        ErrorIllegalOption = 7,

        /// <summary>Will be raised when a PID without a living process was found.</summary>
        ErrorDeadPid = 8,

        /// <summary>Will be raised when hitting an unimplemented feature.</summary>
        ErrorNotImplemented = 9,

        /// <summary>Will be raised when the parameter does not fulfill the requirements.</summary>
        ErrorBadParameter = 10,

        /// <summary>Will be raised when you are missing permission for the operation.</summary>
        ErrorForbidden = 11,

        /// <summary>Will be raised when there is a memory shortage.</summary>
        ErrorOutOfMemoryMmap = 12,

        /// <summary>Will be raised when encountering a corrupt csv line.</summary>
        ErrorCorruptedCsv = 13,

        /// <summary>Will be raised when a file is not found.</summary>
        ErrorFileNotFound = 14,

        /// <summary>Will be raised when a file cannot be written.</summary>
        ErrorCannotWriteFile = 15,

        /// <summary>Will be raised when an attempt is made to overwrite an existing file.</summary>
        ErrorCannotOverwriteFile = 16,

        /// <summary>Will be raised when a type error is encountered.</summary>
        ErrorTypeError = 17,

        /// <summary>Will be raised when there’s a timeout waiting for a lock.</summary>
        ErrorLockTimeout = 18,

        /// <summary>Will be raised when an attempt to create a directory fails.</summary>
        ErrorCannotCreateDirectory = 19,

        /// <summary>Will be raised when an attempt to create a temporary file fails.</summary>
        ErrorCannotCreateTempFile = 20,

        /// <summary>Will be raised when a request is canceled by the user.</summary>
        ErrorRequestCanceled = 21,

        /// <summary>Will be raised intentionally during debugging.</summary>
        ErrorDebug = 22,

        /// <summary>Will be raised when the structure of an IP address is invalid.</summary>
        ErrorIpAddressInvalid = 25,

        /// <summary>Will be raised when a file already exists.</summary>
        ErrorFileExists = 27,

        /// <summary>Will be raised when a resource or an operation is locked.</summary>
        ErrorLocked = 28,

        /// <summary>Will be raised when a deadlock is detected when accessing collections.</summary>
        ErrorDeadlock = 29,

        /// <summary>Will be raised when a call cannot succeed because a server shutdown is already in progress.</summary>
        ErrorShuttingDown = 30,

        /// <summary>Will be raised when an Enterprise Edition feature is requested from the Community Edition.</summary>
        ErrorOnlyEnterprise = 31,

        /// <summary>Will be raised when the resources used by an operation exceed the configured maximum value.</summary>
        ErrorResourceLimit = 32,

        /// <summary>will be raised if icu operations failed</summary>
        ErrorArangoIcuError = 33,

        /// <summary>Will be raised when a file cannot be read.</summary>
        ErrorCannotReadFile = 34,

        /// <summary>Will be raised when a server is running an incompatible version of ArangoDB.</summary>
        ErrorIncompatibleVersion = 35,

        /// <summary>Will be raised when a requested resource is not enabled.</summary>
        ErrorDisabled = 36,
        #endregion

        #region HTTP error status codes
        /// <summary>Will be raised when the HTTP request does not fulfill the requirements.</summary>
        ErrorHttpBadParameter = 400,

        /// <summary>Will be raised when authorization is required but the user is not authorized.</summary>
        ErrorHttpUnauthorized = 401,

        /// <summary>Will be raised when the operation is forbidden.</summary>
        ErrorHttpForbidden = 403,

        /// <summary>Will be raised when an URI is unknown.</summary>
        ErrorHttpNotFound = 404,

        /// <summary>Will be raised when an unsupported HTTP method is used for an operation.</summary>
        ErrorHttpMethodNotAllowed = 405,

        /// <summary>Will be raised when an unsupported HTTP content type is used for an operation</summary>
        ErrorHttpNotAcceptable = 406,

        /// <summary>Will be raised when a precondition for an HTTP request is not met.</summary>
        ErrorHttpPreconditionFailed = 412,

        /// <summary>Will be raised when an internal server is encountered.</summary>
        ErrorHttpServerError = 500,

        /// <summary>Will be raised when a service is temporarily unavailable.</summary>
        ErrorHttpServiceUnavailable = 503,

        /// <summary>Will be raised when a service contacted by ArangoDB does not respond in a timely manner.</summary>
        ErrorHttpGatewayTimeout = 504,
        #endregion

        #region HTTP processing errors
        /// <summary>Will be raised when a string representation of a JSON object is corrupt.</summary>
        ErrorHttpCorruptedJson = 600,

        /// <summary>Will be raised when the URL contains superfluous suffices.</summary>
        ErrorHttpSuperfluousSuffices = 601,
        #endregion

        #region Internal ArangoDB storage errors
        /// <summary>Internal error that will be raised when the datafile is not in the required state.</summary>
        ErrorArangoIllegalState = 1000,

        /// <summary>Internal error that will be raised when trying to write to a datafile.</summary>
        ErrorArangoDatafileSealed = 1002,

        /// <summary>Internal error that will be raised when trying to write to a read-only datafile or collection.</summary>
        ErrorArangoReadOnly = 1004,

        /// <summary>Internal error that will be raised when a identifier duplicate is detected.</summary>
        ErrorArangoDuplicateIdentifier = 1005,

        /// <summary>Internal error that will be raised when a datafile is unreadable.</summary>
        ErrorArangoDatafileUnreadable = 1006,

        /// <summary>Internal error that will be raised when a datafile is empty.</summary>
        ErrorArangoDatafileEmpty = 1007,

        /// <summary>Will be raised when an error occurred during WAL log file recovery.</summary>
        ErrorArangoRecovery = 1008,

        /// <summary>Will be raised when a required datafile statistics object was not found.</summary>
        ErrorArangoDatafileStatisticsNotFound = 1009,
        #endregion

        #region External ArangoDB storage errors
        /// <summary>Will be raised when a corruption is detected in a datafile.</summary>
        ErrorArangoCorruptedDatafile = 1100,

        /// <summary>Will be raised if a parameter file is corrupted or cannot be read.</summary>
        ErrorArangoIllegalParameterFile = 1101,

        /// <summary>Will be raised when a collection contains one or more corrupted data files.</summary>
        ErrorArangoCorruptedCollection = 1102,

        /// <summary>Will be raised when the system call mmap failed.</summary>
        ErrorArangoMmapFailed = 1103,

        /// <summary>Will be raised when the filesystem is full.</summary>
        ErrorArangoFilesystemFull = 1104,

        /// <summary>Will be raised when a journal cannot be created.</summary>
        ErrorArangoNoJournal = 1105,

        /// <summary>Will be raised when the datafile cannot be created or renamed because a file of the same name already exists.</summary>
        ErrorArangoDatafileAlreadyExists = 1106,

        /// <summary>Will be raised when the database directory is locked by a different process.</summary>
        ErrorArangoDatadirLocked = 1107,

        /// <summary>Will be raised when the collection cannot be created because a directory of the same name already exists.</summary>
        ErrorArangoCollectionDirectoryAlreadyExists = 1108,

        /// <summary>Will be raised when the system call msync failed.</summary>
        ErrorArangoMsyncFailed = 1109,

        /// <summary>Will be raised when the server cannot lock the database directory on startup.</summary>
        ErrorArangoDatadirUnlockable = 1110,

        /// <summary>Will be raised when the server waited too long for a datafile to be synced to disk.</summary>
        ErrorArangoSyncTimeout = 1111,
        #endregion

        #region General ArangoDB storage errors
        /// <summary>Will be raised when updating or deleting a document and a conflict has been detected.</summary>
        ErrorArangoConflict = 1200,

        /// <summary>Will be raised when a non-existing database directory was specified when starting the database.</summary>
        ErrorArangoDatadirInvalid = 1201,

        /// <summary>Will be raised when a document with a given identifier is unknown.</summary>
        ErrorArangoDocumentNotFound = 1202,

        /// <summary>Will be raised when a collection or View with the given identifier or name is unknown.</summary>
        ErrorArangoDataSourceNotFound = 1203,

        /// <summary>Will be raised when the collection parameter is missing.</summary>
        ErrorArangoCollectionParameterMissing = 1204,

        /// <summary>Will be raised when a document identifier is corrupt.</summary>
        ErrorArangoDocumentHandleBad = 1205,

        /// <summary>Will be raised when the maximal size of the journal is too small.</summary>
        ErrorArangoMaximalSizeTooSmall = 1206,

        /// <summary>Will be raised when a name duplicate is detected.</summary>
        ErrorArangoDuplicateName = 1207,

        /// <summary>Will be raised when an illegal name is detected.</summary>
        ErrorArangoIllegalName = 1208,

        /// <summary>Will be raised when no suitable index for the query is known.</summary>
        ErrorArangoNoIndex = 1209,

        /// <summary>Will be raised when there is a unique constraint violation.</summary>
        ErrorArangoUniqueConstraintViolated = 1210,

        /// <summary>Will be raised when an index with a given identifier is unknown.</summary>
        ErrorArangoIndexNotFound = 1212,

        /// <summary>Will be raised when a cross-collection is requested.</summary>
        ErrorArangoCrossCollectionRequest = 1213,

        /// <summary>Will be raised when a index identifier is corrupt.</summary>
        ErrorArangoIndexHandleBad = 1214,

        /// <summary>Will be raised when the document cannot fit into any datafile because of it is too large.</summary>
        ErrorArangoDocumentTooLarge = 1216,

        /// <summary>Will be raised when a collection should be unloaded</summary>
        ErrorArangoCollectionNotUnloaded = 1217,

        /// <summary>Will be raised when an invalid collection type is used in a request.</summary>
        ErrorArangoCollectionTypeInvalid = 1218,

        /// <summary>Will be raised when parsing an attribute name definition failed.</summary>
        ErrorArangoAttributeParserFailed = 1220,

        /// <summary>Will be raised when a document key is corrupt.</summary>
        ErrorArangoDocumentKeyBad = 1221,

        /// <summary>Will be raised when a user-defined document key is supplied for collections with auto key generation.</summary>
        ErrorArangoDocumentKeyUnexpected = 1222,

        /// <summary>Will be raised when the server’s database directory is not writable for the current user.</summary>
        ErrorArangoDatadirNotWritable = 1224,

        /// <summary>Will be raised when a key generator runs out of keys.</summary>
        ErrorArangoOutOfKeys = 1225,

        /// <summary>Will be raised when a document key is missing.</summary>
        ErrorArangoDocumentKeyMissing = 1226,

        /// <summary>Will be raised when there is an attempt to create a document with an invalid type.</summary>
        ErrorArangoDocumentTypeInvalid = 1227,

        /// <summary>Will be raised when a non-existing database is accessed.</summary>
        ErrorArangoDatabaseNotFound = 1228,

        /// <summary>Will be raised when an invalid database name is used.</summary>
        ErrorArangoDatabaseNameInvalid = 1229,

        /// <summary>Will be raised when an operation is requested in a database other than the system database.</summary>
        ErrorArangoUseSystemDatabase = 1230,

        /// <summary>Will be raised when an invalid key generator description is used.</summary>
        ErrorArangoInvalidKeyGenerator = 1232,

        /// <summary>will be raised when the from or to values of an edge are undefined or contain an invalid value.</summary>
        ErrorArangoInvalidEdgeAttribute = 1233,

        /// <summary>Will be raised when an attempt to create an index has failed.</summary>
        ErrorArangoIndexCreationFailed = 1235,

        /// <summary>Will be raised when the server is write-throttled and a write operation has waited too long for the server to process queued operations.</summary>
        ErrorArangoWriteThrottleTimeout = 1236,

        /// <summary>Will be raised when a collection has a different type from what has been expected.</summary>
        ErrorArangoCollectionTypeMismatch = 1237,

        /// <summary>Will be raised when a collection is accessed that is not yet loaded.</summary>
        ErrorArangoCollectionNotLoaded = 1238,

        /// <summary>Will be raised when a document revision is corrupt or is missing where needed.</summary>
        ErrorArangoDocumentRevBad = 1239,

        /// <summary>Will be raised by the storage engine when a read cannot be completed.</summary>
        ErrorArangoIncompleteRead = 1240,
        #endregion

        #region Checked ArangoDB storage errors
        /// <summary>Will be raised when the datafile reaches its limit.</summary>
        ErrorArangoDatafileFull = 1300,

        /// <summary>Will be raised when encountering an empty server database directory.</summary>
        ErrorArangoEmptyDatadir = 1301,

        /// <summary>Will be raised when an operation should be retried.</summary>
        ErrorArangoTryAgain = 1302,

        /// <summary>Will be raised when storage engine is busy.</summary>
        ErrorArangoBusy = 1303,

        /// <summary>Will be raised when storage engine has a datafile merge in progress and cannot complete the operation.</summary>
        ErrorArangoMergeInProgress = 1304,

        /// <summary>Will be raised when storage engine encounters an I/O error.</summary>
        ErrorArangoIoError = 1305,
        #endregion

        #region ArangoDB replication errors
        /// <summary>Will be raised when the replication applier does not receive any or an incomplete response from the master.</summary>
        ErrorReplicationNoResponse = 1400,

        /// <summary>Will be raised when the replication applier receives an invalid response from the master.</summary>
        ErrorReplicationInvalidResponse = 1401,

        /// <summary>Will be raised when the replication applier receives a server error from the master.</summary>
        ErrorReplicationMasterError = 1402,

        /// <summary>Will be raised when the replication applier connects to a master that has an incompatible version.</summary>
        ErrorReplicationMasterIncompatible = 1403,

        /// <summary>Will be raised when the replication applier connects to a different master than before.</summary>
        ErrorReplicationMasterChange = 1404,

        /// <summary>Will be raised when the replication applier is asked to connect to itself for replication.</summary>
        ErrorReplicationLoop = 1405,

        /// <summary>Will be raised when an unexpected marker is found in the replication log stream.</summary>
        ErrorReplicationUnexpectedMarker = 1406,

        /// <summary>Will be raised when an invalid replication applier state file is found.</summary>
        ErrorReplicationInvalidApplierState = 1407,

        /// <summary>Will be raised when an unexpected transaction id is found.</summary>
        ErrorReplicationUnexpectedTransaction = 1408,

        /// <summary>Will be raised when the configuration for the replication applier is invalid.</summary>
        ErrorReplicationInvalidApplierConfiguration = 1410,

        /// <summary>Will be raised when there is an attempt to perform an operation while the replication applier is running.</summary>
        ErrorReplicationRunning = 1411,

        /// <summary>Special error code used to indicate the replication applier was stopped by a user.</summary>
        ErrorReplicationApplierStopped = 1412,

        /// <summary>Will be raised when the replication applier is started without a known start tick value.</summary>
        ErrorReplicationNoStartTick = 1413,

        /// <summary>Will be raised when the replication applier fetches data using a start tick</summary>
        ErrorReplicationStartTickNotPresent = 1414,

        /// <summary>Will be raised when a new born follower submits a wrong checksum</summary>
        ErrorReplicationWrongChecksum = 1416,

        /// <summary>Will be raised when a shard is not empty and the follower tries a shortcut</summary>
        ErrorReplicationShardNonempty = 1417,
        #endregion

        #region ArangoDB cluster errors
        /// <summary>Will be raised when updating the plan on collection creatio failed.</summary>
        ErrorClusterCreateCollectionPreconditionFailed = 1448,

        /// <summary>Will be raised on some occasions when one server gets a request from another</summary>
        ErrorClusterServerUnknown = 1449,

        /// <summary>Will be raised when the number of shards for a collection is higher than allowed.</summary>
        ErrorClusterTooManyShards = 1450,

        /// <summary>Will be raised when a Coordinator in a cluster tries to create a collection and the collection ID already exists.</summary>
        ErrorClusterCollectionIdExists = 1453,

        /// <summary>Will be raised when a Coordinator in a cluster cannot create an entry for a new collection in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotCreateCollectionInPlan = 1454,

        /// <summary>Will be raised when a Coordinator in a cluster notices that some DB-Servers report problems when creating shards for a new collection.</summary>
        ErrorClusterCouldNotCreateCollection = 1456,

        /// <summary>Will be raised when a Coordinator in a cluster runs into a timeout for some cluster wide operation.</summary>
        ErrorClusterTimeout = 1457,

        /// <summary>Will be raised when a Coordinator in a cluster cannot remove an entry for a collection in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotRemoveCollectionInPlan = 1458,

        /// <summary>Will be raised when a Coordinator in a cluster cannot remove an entry for a collection in the Current hierarchy in the Agency.</summary>
        ErrorClusterCouldNotRemoveCollectionInCurrent = 1459,

        /// <summary>Will be raised when a Coordinator in a cluster cannot create an entry for a new database in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotCreateDatabaseInPlan = 1460,

        /// <summary>Will be raised when a Coordinator in a cluster notices that some DB-Servers report problems when creating databases for a new cluster wide database.</summary>
        ErrorClusterCouldNotCreateDatabase = 1461,

        /// <summary>Will be raised when a Coordinator in a cluster cannot remove an entry for a database in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotRemoveDatabaseInPlan = 1462,

        /// <summary>Will be raised when a Coordinator in a cluster cannot remove an entry for a database in the Current hierarchy in the Agency.</summary>
        ErrorClusterCouldNotRemoveDatabaseInCurrent = 1463,

        /// <summary>Will be raised when a Coordinator in a cluster cannot determine the shard that is responsible for a given document.</summary>
        ErrorClusterShardGone = 1464,

        /// <summary>Will be raised when a Coordinator in a cluster loses an HTTP connection to a DB-Server in the cluster whilst transferring data.</summary>
        ErrorClusterConnectionLost = 1465,

        /// <summary>Will be raised when a Coordinator in a cluster finds that the key attribute was specified in a sharded collection the uses not only key as sharding attribute.</summary>
        ErrorClusterMustNotSpecifyKey = 1466,

        /// <summary>Will be raised if a Coordinator in a cluster gets conflicting results from different shards</summary>
        ErrorClusterGotContradictingAnswers = 1467,

        /// <summary>Will be raised if a Coordinator tries to find out which shard is responsible for a partial document</summary>
        ErrorClusterNotAllShardingAttributesGiven = 1468,

        /// <summary>Will be raised if there is an attempt to update the value of a shard attribute.</summary>
        ErrorClusterMustNotChangeShardingAttributes = 1469,

        /// <summary>Will be raised when there is an attempt to carry out an operation that is not supported in the context of a sharded collection.</summary>
        ErrorClusterUnsupported = 1470,

        /// <summary>Will be raised if there is an attempt to run a Coordinator-only operation on a different type of node.</summary>
        ErrorClusterOnlyOnCoordinator = 1471,

        /// <summary>Will be raised if a Coordinator or DB-Server cannot read the Plan in the Agency.</summary>
        ErrorClusterReadingPlanAgency = 1472,

        /// <summary>Will be raised if a Coordinator cannot truncate all shards of a cluster collection.</summary>
        ErrorClusterCouldNotTruncateCollection = 1473,

        /// <summary>Will be raised if the internal communication of the cluster for AQL produces an error.</summary>
        ErrorClusterAqlCommunication = 1474,

        /// <summary>Will be raised if there is an attempt to run a DB-Server-only operation on a different type of node.</summary>
        ErrorClusterOnlyOnDbserver = 1477,

        /// <summary>Will be raised if a required DB-Server can’t be reached.</summary>
        ErrorClusterBackendUnavailable = 1478,

        /// <summary>Will be raised if a collection needed during query execution is out of sync. This currently can only happen when using SatelliteCollections</summary>
        ErrorClusterAqlCollectionOutOfSync = 1481,

        /// <summary>Will be raised when a Coordinator in a cluster cannot create an entry for a new index in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotCreateIndexInPlan = 1482,

        /// <summary>Will be raised when a Coordinator in a cluster cannot remove an index from the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotDropIndexInPlan = 1483,

        /// <summary>Will be raised if one tries to create a collection with a distributeShardsLike attribute which points to another collection that also has one.</summary>
        ErrorClusterChainOfDistributeshardslike = 1484,

        /// <summary>Will be raised if one tries to drop a collection to which another collection points with its distributeShardsLike attribute.</summary>
        ErrorClusterMustNotDropCollOtherDistributeshardslike = 1485,

        /// <summary>Will be raised if one tries to create a collection which points to an unknown collection in its distributeShardsLike attribute.</summary>
        ErrorClusterUnknownDistributeshardslike = 1486,

        /// <summary>Will be raised if one tries to create a collection with a replicationFactor greater than the available number of DB-Servers.</summary>
        ErrorClusterInsufficientDbservers = 1487,

        /// <summary>Will be raised if a follower that ought to be dropped could not be dropped in the Agency (under Current).</summary>
        ErrorClusterCouldNotDropFollower = 1488,

        /// <summary>Will be raised if a replication operation is refused by a shard leader.</summary>
        ErrorClusterShardLeaderRefusesReplication = 1489,

        /// <summary>Will be raised if a non-replication operation is refused by a shard follower.</summary>
        ErrorClusterShardFollowerRefusesOperation = 1490,

        /// <summary>because it has resigned in the meantime</summary>
        ErrorClusterShardLeaderResigned = 1491,

        /// <summary>Will be raised if after various retries an Agency operation could not be performed successfully.</summary>
        ErrorClusterAgencyCommunicationFailed = 1492,

        /// <summary>Will be raised when servers are currently competing for leadership</summary>
        ErrorClusterLeadershipChallengeOngoing = 1495,

        /// <summary>Will be raised when an operation is sent to a non-leading server.</summary>
        ErrorClusterNotLeader = 1496,

        /// <summary>Will be raised when a Coordinator in a cluster cannot create an entry for a new View in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotCreateViewInPlan = 1497,

        /// <summary>Will be raised when a Coordinator in a cluster tries to create a View and the View ID already exists.</summary>
        ErrorClusterViewIdExists = 1498,

        /// <summary>Will be raised when a Coordinator in a cluster cannot drop a collection entry in the Plan hierarchy in the Agency.</summary>
        ErrorClusterCouldNotDropCollection = 1499,
        #endregion

        #region ArangoDB query errors
        /// <summary>Will be raised when a running query is killed by an explicit admin command.</summary>
        ErrorQueryKilled = 1500,

        /// <summary>Will be raised when query is parsed and is found to be syntactically invalid.</summary>
        ErrorQueryParse = 1501,

        /// <summary>Will be raised when an empty query is specified.</summary>
        ErrorQueryEmpty = 1502,

        /// <summary>Will be raised when a runtime error is caused by the query.</summary>
        ErrorQueryScript = 1503,

        /// <summary>Will be raised when a number is outside the expected range.</summary>
        ErrorQueryNumberOutOfRange = 1504,

        /// <summary>Will be raised when a geo index coordinate is invalid or out of range.</summary>
        ErrorQueryInvalidGeoValue = 1505,

        /// <summary>Will be raised when an invalid variable name is used.</summary>
        ErrorQueryVariableNameInvalid = 1510,

        /// <summary>Will be raised when a variable gets re-assigned in a query.</summary>
        ErrorQueryVariableRedeclared = 1511,

        /// <summary>Will be raised when an unknown variable is used or the variable is undefined the context it is used.</summary>
        ErrorQueryVariableNameUnknown = 1512,

        /// <summary>Will be raised when a read lock on the collection cannot be acquired.</summary>
        ErrorQueryCollectionLockFailed = 1521,

        /// <summary>Will be raised when the number of collections or shards in a query is beyond the allowed value.</summary>
        ErrorQueryTooManyCollections = 1522,

        /// <summary>Will be raised when a document attribute is re-assigned.</summary>
        ErrorQueryDocumentAttributeRedeclared = 1530,

        /// <summary>Will be raised when an undefined function is called.</summary>
        ErrorQueryFunctionNameUnknown = 1540,

        /// <summary>expected number of arguments: minimum: %d</summary>
        ErrorQueryFunctionArgumentNumberMismatch = 1541,

        /// <summary>Will be raised when the type of an argument used in a function call does not match the expected argument type.</summary>
        ErrorQueryFunctionArgumentTypeMismatch = 1542,

        /// <summary>Will be raised when an invalid regex argument value is used in a call to a function that expects a regex.</summary>
        ErrorQueryInvalidRegex = 1543,

        /// <summary>Will be raised when the structure of bind parameters passed has an unexpected format.</summary>
        ErrorQueryBindParametersInvalid = 1550,

        /// <summary>Will be raised when a bind parameter was declared in the query but the query is being executed with no value for that parameter.</summary>
        ErrorQueryBindParameterMissing = 1551,

        /// <summary>Will be raised when a value gets specified for an undeclared bind parameter.</summary>
        ErrorQueryBindParameterUndeclared = 1552,

        /// <summary>Will be raised when a bind parameter has an invalid value or type.</summary>
        ErrorQueryBindParameterType = 1553,

        /// <summary>Will be raised when a non-boolean value is used in a logical operation.</summary>
        ErrorQueryInvalidLogicalValue = 1560,

        /// <summary>Will be raised when a non-numeric value is used in an arithmetic operation.</summary>
        ErrorQueryInvalidArithmeticValue = 1561,

        /// <summary>Will be raised when there is an attempt to divide by zero.</summary>
        ErrorQueryDivisionByZero = 1562,

        /// <summary>Will be raised when a non-array operand is used for an operation that expects an array argument operand.</summary>
        ErrorQueryArrayExpected = 1563,

        /// <summary>Will be raised when the function FAIL() is called from inside a query.</summary>
        ErrorQueryFailCalled = 1569,

        /// <summary>Will be raised when a geo restriction was specified but no suitable geo index is found to resolve it.</summary>
        ErrorQueryGeoIndexMissing = 1570,

        /// <summary>Will be raised when a fulltext query is performed on a collection without a suitable fulltext index.</summary>
        ErrorQueryFulltextIndexMissing = 1571,

        /// <summary>Will be raised when a value cannot be converted to a date.</summary>
        ErrorQueryInvalidDateValue = 1572,

        /// <summary>Will be raised when an AQL query contains more than one data-modifying operation.</summary>
        ErrorQueryMultiModify = 1573,

        /// <summary>Will be raised when an AQL query contains an invalid aggregate expression.</summary>
        ErrorQueryInvalidAggregateExpression = 1574,

        /// <summary>Will be raised when an AQL data-modification query contains options that cannot be figured out at query compile time.</summary>
        ErrorQueryCompileTimeOptions = 1575,

        /// <summary>Will be raised when an AQL data-modification query contains an invalid options specification.</summary>
        ErrorQueryExceptionOptions = 1576,

        /// <summary>Will be raised when forceIndexHint is specified</summary>
        ErrorQueryForcedIndexHintUnusable = 1577,

        /// <summary>Will be raised when a dynamic function call is made to a function that cannot be called dynamically.</summary>
        ErrorQueryDisallowedDynamicCall = 1578,

        /// <summary>Will be raised when collection data are accessed after a data-modification operation.</summary>
        ErrorQueryAccessAfterModification = 1579,
        #endregion

        #region AQL user function errors
        /// <summary>Will be raised when a user function with an invalid name is registered.</summary>
        ErrorQueryFunctionInvalidName = 1580,

        /// <summary>Will be raised when a user function is registered with invalid code.</summary>
        ErrorQueryFunctionInvalidCode = 1581,

        /// <summary>Will be raised when a user function is accessed but not found.</summary>
        ErrorQueryFunctionNotFound = 1582,

        /// <summary>Will be raised when a user function throws a runtime exception.</summary>
        ErrorQueryFunctionRuntimeError = 1583,
        #endregion

        #region AQL query registry errors
        /// <summary>Will be raised when an HTTP API for a query got an invalid JSON object.</summary>
        ErrorQueryBadJsonPlan = 1590,

        /// <summary>Will be raised when an Id of a query is not found by the HTTP API.</summary>
        ErrorQueryNotFound = 1591,

        /// <summary>Will be raised if and user provided expression fails to evaluate to true</summary>
        ErrorQueryUserAssert = 1593,

        /// <summary>Will be raised if and user provided expression fails to evaluate to true</summary>
        ErrorQueryUserWarn = 1594,
        #endregion

        #region ArangoDB cursor errors
        /// <summary>Will be raised when a cursor is requested via its id but a cursor with that id cannot be found.</summary>
        ErrorCursorNotFound = 1600,

        /// <summary>Will be raised when a cursor is requested via its id but a concurrent request is still using the cursor.</summary>
        ErrorCursorBusy = 1601,
        #endregion

        #region ArangoDB schema validation errors
        /// <summary>Will be raised when a document does not pass schema validation.</summary>
        ErrorValidationFailed = 1620,

        /// <summary>Will be raised when the schema description is invalid.</summary>
        ErrorValidationBadParameter = 1621,
        #endregion

        #region ArangoDB transaction errors
        /// <summary>Will be raised when a wrong usage of transactions is detected. this is an internal error and indicates a bug in ArangoDB.</summary>
        ErrorTransactionInternal = 1650,

        /// <summary>Will be raised when transactions are nested.</summary>
        ErrorTransactionNested = 1651,

        /// <summary>Will be raised when a collection is used in the middle of a transaction but was not registered at transaction start.</summary>
        ErrorTransactionUnregisteredCollection = 1652,

        /// <summary>Will be raised when a disallowed operation is carried out in a transaction.</summary>
        ErrorTransactionDisallowedOperation = 1653,

        /// <summary>Will be raised when a transaction was aborted.</summary>
        ErrorTransactionAborted = 1654,

        /// <summary>Will be raised when a transaction was not found.</summary>
        ErrorTransactionNotFound = 1655,
        #endregion

        #region User management errors
        /// <summary>Will be raised when an invalid user name is used.</summary>
        ErrorUserInvalidName = 1700,

        /// <summary>Will be raised when a user name already exists.</summary>
        ErrorUserDuplicate = 1702,

        /// <summary>Will be raised when a user name is updated that does not exist.</summary>
        ErrorUserNotFound = 1703,

        /// <summary>Will be raised when the user is authenticated by an external server.</summary>
        ErrorUserExternal = 1705,
        #endregion

        #region Service management errors (legacy)
        /// <summary>Will be raised when a service download from the central repository failed.</summary>
        ErrorServiceDownloadFailed = 1752,

        /// <summary>Will be raised when a service upload from the client to the ArangoDB server failed.</summary>
        ErrorServiceUploadFailed = 1753,
        #endregion

        #region LDAP errors
        /// <summary>can not init a LDAP connection</summary>
        ErrorLdapCannotInit = 1800,

        /// <summary>can not set a LDAP option</summary>
        ErrorLdapCannotSetOption = 1801,

        /// <summary>can not bind to a LDAP server</summary>
        ErrorLdapCannotBind = 1802,

        /// <summary>can not unbind from a LDAP server</summary>
        ErrorLdapCannotUnbind = 1803,

        /// <summary>can not search the LDAP server</summary>
        ErrorLdapCannotSearch = 1804,

        /// <summary>can not star a TLS LDAP session</summary>
        ErrorLdapCannotStartTls = 1805,

        /// <summary>LDAP didn’t found any objects with the specified search query</summary>
        ErrorLdapFoundNoObjects = 1806,

        /// <summary>LDAP found zero ore more than one user</summary>
        ErrorLdapNotOneUserFound = 1807,

        /// <summary>but its not the desired one</summary>
        ErrorLdapUserNotIdentified = 1808,

        /// <summary>LDAP returned an operations error</summary>
        ErrorLdapOperationsError = 1809,

        /// <summary>cant distinguish a valid mode for provided LDAP configuration</summary>
        ErrorLdapInvalidMode = 1820,
        #endregion

        #region Task errors
        /// <summary>Will be raised when a task is created with an invalid id.</summary>
        ErrorTaskInvalidId = 1850,

        /// <summary>Will be raised when a task id is created with a duplicate id.</summary>
        ErrorTaskDuplicateId = 1851,

        /// <summary>Will be raised when a task with the specified id could not be found.</summary>
        ErrorTaskNotFound = 1852,
        #endregion

        #region Graph / traversal errors
        /// <summary>Will be raised when an invalid name is passed to the server.</summary>
        ErrorGraphInvalidGraph = 1901,

        /// <summary>Will be raised when an invalid name</summary>
        ErrorGraphCouldNotCreateGraph = 1902,

        /// <summary>Will be raised when an invalid vertex id is passed to the server.</summary>
        ErrorGraphInvalidVertex = 1903,

        /// <summary>Will be raised when the vertex could not be created.</summary>
        ErrorGraphCouldNotCreateVertex = 1904,

        /// <summary>Will be raised when the vertex could not be changed.</summary>
        ErrorGraphCouldNotChangeVertex = 1905,

        /// <summary>Will be raised when an invalid edge id is passed to the server.</summary>
        ErrorGraphInvalidEdge = 1906,

        /// <summary>Will be raised when the edge could not be created.</summary>
        ErrorGraphCouldNotCreateEdge = 1907,

        /// <summary>Will be raised when the edge could not be changed.</summary>
        ErrorGraphCouldNotChangeEdge = 1908,

        /// <summary>Will be raised when too many iterations are done in a graph traversal.</summary>
        ErrorGraphTooManyIterations = 1909,

        /// <summary>Will be raised when an invalid filter result is returned in a graph traversal.</summary>
        ErrorGraphInvalidFilterResult = 1910,

        /// <summary>an edge collection may only be used once in one edge definition of a graph.</summary>
        ErrorGraphCollectionMultiUse = 1920,

        /// <summary>is already used by another graph in a different edge definition.</summary>
        ErrorGraphCollectionUseInMultiGraphs = 1921,

        /// <summary>a graph name is required to create or drop a graph.</summary>
        ErrorGraphCreateMissingName = 1922,

        /// <summary>the edge definition is malformed. It has to be an array of objects.</summary>
        ErrorGraphCreateMalformedEdgeDefinition = 1923,

        /// <summary>a graph with this name could not be found.</summary>
        ErrorGraphNotFound = 1924,

        /// <summary>a graph with this name already exists.</summary>
        ErrorGraphDuplicate = 1925,

        /// <summary>the specified vertex collection does not exist or is not part of the graph.</summary>
        ErrorGraphVertexColDoesNotExist = 1926,

        /// <summary>the collection is not a vertex collection.</summary>
        ErrorGraphWrongCollectionTypeVertex = 1927,

        /// <summary>Vertex collection not in list of orphan collections of the graph.</summary>
        ErrorGraphNotInOrphanCollection = 1928,

        /// <summary>The collection is already used in an edge definition of the graph.</summary>
        ErrorGraphCollectionUsedInEdgeDef = 1929,

        /// <summary>The edge collection is not used in any edge definition of the graph.</summary>
        ErrorGraphEdgeCollectionNotUsed = 1930,

        /// <summary>collection graphs does not exist.</summary>
        ErrorGraphNoGraphCollection = 1932,

        /// <summary>Array or Object</summary>
        ErrorGraphInvalidExampleArrayObjectString = 1933,

        /// <summary>Invalid example type. Has to be Array or Object.</summary>
        ErrorGraphInvalidExampleArrayObject = 1934,

        /// <summary>Invalid number of arguments. Expected:</summary>
        ErrorGraphInvalidNumberOfArguments = 1935,

        /// <summary>Invalid parameter type.</summary>
        ErrorGraphInvalidParameter = 1936,

        /// <summary>Invalid id</summary>
        ErrorGraphInvalidId = 1937,

        /// <summary>The collection is already used in the orphans of the graph.</summary>
        ErrorGraphCollectionUsedInOrphans = 1938,

        /// <summary>the specified edge collection does not exist or is not part of the graph.</summary>
        ErrorGraphEdgeColDoesNotExist = 1939,

        /// <summary>The requested graph has no edge collections.</summary>
        ErrorGraphEmpty = 1940,

        /// <summary>The graphs collection contains invalid data.</summary>
        ErrorGraphInternalDataCorrupt = 1941,

        /// <summary>Tried to add an edge collection which is already defined.</summary>
        ErrorGraphInternalEdgeCollectionAlreadySet = 1942,

        /// <summary>the orphan list argument is malformed. It has to be an array of strings.</summary>
        ErrorGraphCreateMalformedOrphanList = 1943,

        /// <summary>the collection used as a relation is existing</summary>
        ErrorGraphEdgeDefinitionIsDocument = 1944,

        /// <summary>the collection is used as the initial collection of this graph and is not allowed to be removed manually.</summary>
        ErrorGraphCollectionIsInitial = 1945,

        /// <summary>during the graph creation process no collection could be selected as the needed initial collection. Happens if a distributeShardsLike or replicationFactor mismatch was found.</summary>
        ErrorGraphNoInitialCollection = 1946,
        #endregion

        #region Session errors
        /// <summary>Will be raised when an invalid/unknown session id is passed to the server.</summary>
        ErrorSessionUnknown = 1950,

        /// <summary>Will be raised when a session is expired.</summary>
        ErrorSessionExpired = 1951,
        #endregion

        #region Simple Client errors
        /// <summary>This error should not happen.</summary>
        ErrorSimpleClientUnknownError = 2000,

        /// <summary>Will be raised when the client could not connect to the server.</summary>
        ErrorSimpleClientCouldNotConnect = 2001,

        /// <summary>Will be raised when the client could not write data.</summary>
        ErrorSimpleClientCouldNotWrite = 2002,

        /// <summary>Will be raised when the client could not read data.</summary>
        ErrorSimpleClientCouldNotRead = 2003,

        /// <summary>Will be raised if was erlaube?!</summary>
        ErrorWasErlaube = 2019,
        #endregion

        #region Internal AQL errors
        /// <summary>Internal error during AQL execution</summary>
        ErrorInternalAql = 2200,

        /// <summary>An AQL block wrote too few output registers</summary>
        ErrorWroteTooFewOutputRegisters = 2201,

        /// <summary>An AQL block wrote too many output registers</summary>
        ErrorWroteTooManyOutputRegisters = 2202,

        /// <summary>An AQL block wrote an output register twice</summary>
        ErrorWroteOutputRegisterTwice = 2203,

        /// <summary>An AQL block wrote in a register that is not its output</summary>
        ErrorWroteInWrongRegister = 2204,

        /// <summary>An AQL block did not copy its input registers</summary>
        ErrorInputRegistersNotCopied = 2205,
        #endregion

        #region Foxx management errors
        /// <summary>The service manifest file is not well-formed JSON.</summary>
        ErrorMalformedManifestFile = 3000,

        /// <summary>The service manifest contains invalid values.</summary>
        ErrorInvalidServiceManifest = 3001,

        /// <summary>The service folder or bundle does not exist on this server.</summary>
        ErrorServiceFilesMissing = 3002,

        /// <summary>The local service bundle does not match the checksum in the database.</summary>
        ErrorServiceFilesOutdated = 3003,

        /// <summary>The service options contain invalid values.</summary>
        ErrorInvalidFoxxOptions = 3004,

        /// <summary>The service mountpath contains invalid characters.</summary>
        ErrorInvalidMountpoint = 3007,

        /// <summary>No service found at the given mountpath.</summary>
        ErrorServiceNotFound = 3009,

        /// <summary>The service is missing configuration or dependencies.</summary>
        ErrorServiceNeedsConfiguration = 3010,

        /// <summary>A service already exists at the given mountpath.</summary>
        ErrorServiceMountpointConflict = 3011,

        /// <summary>The service directory does not contain a manifest file.</summary>
        ErrorServiceManifestNotFound = 3012,

        /// <summary>The service options are not well-formed JSON.</summary>
        ErrorServiceOptionsMalformed = 3013,

        /// <summary>The source path does not match a file or directory.</summary>
        ErrorServiceSourceNotFound = 3014,

        /// <summary>The source path could not be resolved.</summary>
        ErrorServiceSourceError = 3015,

        /// <summary>The service does not have a script with this name.</summary>
        ErrorServiceUnknownScript = 3016,

        /// <summary>The API for managing Foxx services has been disabled on this server.</summary>
        ErrorServiceApiDisabled = 3099,
        #endregion

        #region JavaScript module loader errors
        /// <summary>The module path could not be resolved.</summary>
        ErrorModuleNotFound = 3100,

        /// <summary>The module could not be parsed because of a syntax error.</summary>
        ErrorModuleSyntaxError = 3101,

        /// <summary>Failed to invoke the module in its context.</summary>
        ErrorModuleFailure = 3103,
        #endregion

        #region Enterprise Edition errors
        /// <summary>The requested collection needs to be smart</summary>
        ErrorNoSmartCollection = 4000,

        /// <summary>The given document does not have the SmartGraph attribute set.</summary>
        ErrorNoSmartGraphAttribute = 4001,

        /// <summary>This smart collection cannot be dropped</summary>
        ErrorCannotDropSmartCollection = 4002,

        /// <summary>In a smart vertex collection key must be prefixed with the value of the SmartGraph attribute.</summary>
        ErrorKeyMustBePrefixedWithSmartGraphAttribute = 4003,

        /// <summary>The given smartGraph attribute is illegal and cannot be used for sharding. All system attributes are forbidden.</summary>
        ErrorIllegalSmartGraphAttribute = 4004,

        /// <summary>The SmartGraph attribute of the given collection does not match the SmartGraph attribute of the graph.</summary>
        ErrorSmartGraphAttributeMismatch = 4005,

        /// <summary>Will be raised when the smartJoinAttribute declaration is invalid.</summary>
        ErrorInvalidSmartJoinAttribute = 4006,

        /// <summary>when using smartJoinAttribute for a collection</summary>
        ErrorKeyMustBePrefixedWithSmartJoinAttribute = 4007,

        /// <summary>The given document does not have the required SmartJoin attribute set or it has an invalid value.</summary>
        ErrorNoSmartJoinAttribute = 4008,

        /// <summary>Will be raised if there is an attempt to update the value of the smartJoinAttribute.</summary>
        ErrorClusterMustNotChangeSmartJoinAttribute = 4009,

        /// <summary>Will be raised if there is an attempt to create an edge between separated graph components.</summary>
        ErrorInvalidDisjointSmartEdge = 4010,
        #endregion

        #region Cluster repair errors
        /// <summary>General error during cluster repairs</summary>
        ErrorClusterRepairsFailed = 5000,

        /// <summary>Will be raised when</summary>
        ErrorClusterRepairsNotEnoughHealthy = 5001,

        /// <summary>Will be raised on various inconsistencies regarding the replication factor</summary>
        ErrorClusterRepairsReplicationFactorViolated = 5002,

        /// <summary>Will be raised if a collection that is fixed has some shard without DB-Servers</summary>
        ErrorClusterRepairsNoDbservers = 5003,

        /// <summary>Will be raised if a shard in collection and its prototype in the corresponding distributeShardsLike collection have mismatching leaders (when they should already have been fixed)</summary>
        ErrorClusterRepairsMismatchingLeaders = 5004,

        /// <summary>Will be raised if a shard in collection and its prototype in the corresponding distributeShardsLike collection don’t have the same followers (when they should already have been adjusted)</summary>
        ErrorClusterRepairsMismatchingFollowers = 5005,

        /// <summary>Will be raised if a collection that is fixed does (not) have distributeShardsLike when it is expected</summary>
        ErrorClusterRepairsInconsistentAttributes = 5006,

        /// <summary>Will be raised if in a collection and its distributeShardsLike prototype collection some shard and its prototype have an unequal number of DB-Servers</summary>
        ErrorClusterRepairsMismatchingShards = 5007,

        /// <summary>Will be raised if a move shard job in the Agency failed during cluster repairs</summary>
        ErrorClusterRepairsJobFailed = 5008,

        /// <summary>Will be raised if a move shard job in the Agency cannot be found anymore before it finished</summary>
        ErrorClusterRepairsJobDisappeared = 5009,

        /// <summary>Will be raised if an agency transaction failed during either sending or executing it.</summary>
        ErrorClusterRepairsOperationFailed = 5010,
        #endregion

        #region Agency errors
        /// <summary>The inform message in the Agency must be an object.</summary>
        ErrorAgencyInformMustBeObject = 20011,

        /// <summary>The inform message in the Agency must contain a uint parameter ‘term’.</summary>
        ErrorAgencyInformMustContainTerm = 20012,

        /// <summary>The inform message in the Agency must contain a string parameter ‘id’.</summary>
        ErrorAgencyInformMustContainId = 20013,

        /// <summary>The inform message in the Agency must contain an array ‘active’.</summary>
        ErrorAgencyInformMustContainActive = 20014,

        /// <summary>The inform message in the Agency must contain an object ‘pool’.</summary>
        ErrorAgencyInformMustContainPool = 20015,

        /// <summary>The inform message in the Agency must contain an object ‘min ping’.</summary>
        ErrorAgencyInformMustContainMinPing = 20016,

        /// <summary>The inform message in the Agency must contain an object ‘max ping’.</summary>
        ErrorAgencyInformMustContainMaxPing = 20017,

        /// <summary>The inform message in the Agency must contain an object ‘timeoutMult’.</summary>
        ErrorAgencyInformMustContainTimeoutMult = 20018,

        /// <summary>Will be raised if the readDB or the spearHead cannot be rebuilt from the replicated log.</summary>
        ErrorAgencyCannotRebuildDbs = 20021,
        #endregion

        #region Supervision errors
        /// <summary>General supervision failure.</summary>
        ErrorSupervisionGeneralFailure = 20501,
        #endregion

        #region Dispatcher errors
        /// <summary>Will be returned if a queue with this name is full.</summary>
        ErrorQueueFull = 21003,
        #endregion

        #region Maintenance errors
        /// <summary>This maintenance action cannot be stopped once it is started</summary>
        ErrorActionOperationUnabortable = 6002,

        /// <summary>This maintenance action is still processing</summary>
        ErrorActionUnfinished = 6003,

        /// <summary>No such maintenance action exists</summary>
        ErrorNoSuchAction = 6004,
        #endregion

        #region Backup/Restore errors
        /// <summary>Failed to create hot backup set</summary>
        ErrorHotBackupInternal = 7001,

        /// <summary>Failed to restore to hot backup set</summary>
        ErrorHotRestoreInternal = 7002,

        /// <summary>The hot backup set cannot be restored on non matching cluster topology</summary>
        ErrorBackupTopology = 7003,

        /// <summary>No space left on device</summary>
        ErrorNoSpaceLeftOnDevice = 7004,

        /// <summary>Failed to upload hot backup set to remote target</summary>
        ErrorFailedToUploadBackup = 7005,

        /// <summary>Failed to download hot backup set from remote source</summary>
        ErrorFailedToDownloadBackup = 7006,

        /// <summary>Cannot find a hot backup set with this Id</summary>
        ErrorNoSuchHotBackup = 7007,

        /// <summary>The configuration given for upload or download operation to/from remote hot backup repositories is wrong.</summary>
        ErrorRemoteRepositoryConfigBad = 7008,

        /// <summary>Some of the DB-Servers cannot be reached for transaction locks.</summary>
        ErrorLocalLockFailed = 7009,

        /// <summary>Some of the DB-Servers cannot be reached for transaction locks.</summary>
        ErrorLocalLockRetry = 7010,

        /// <summary>Conflict of multiple hot backup processes.</summary>
        ErrorHotBackupConflict = 7011,

        /// <summary>One or more DB-Servers could not be reached for hot backup inquiry</summary>
        ErrorHotBackupDbserversAwol = 7012,
        #endregion

        #region Plan Analyzers errors
        /// <summary>Plan could not be modified while creating or deleting Analyzers revision</summary>
        ErrorClusterCouldNotModifyAnalyzersInPlan = 7021,
        #endregion
    }
}