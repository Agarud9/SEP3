// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: contract.proto

package mango.sep3.databaseaccess.protobuf;

public final class Contract {
  private Contract() {}
  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistryLite registry) {
  }

  public static void registerAllExtensions(
      com.google.protobuf.ExtensionRegistry registry) {
    registerAllExtensions(
        (com.google.protobuf.ExtensionRegistryLite) registry);
  }
  static final com.google.protobuf.Descriptors.Descriptor
    internal_static_Farm_descriptor;
  static final 
    com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internal_static_Farm_fieldAccessorTable;
  static final com.google.protobuf.Descriptors.Descriptor
    internal_static_Offer_descriptor;
  static final 
    com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internal_static_Offer_fieldAccessorTable;

  public static com.google.protobuf.Descriptors.FileDescriptor
      getDescriptor() {
    return descriptor;
  }
  private static  com.google.protobuf.Descriptors.FileDescriptor
      descriptor;
  static {
    java.lang.String[] descriptorData = {
      "\n\016contract.proto\"\253\001\n\004Farm\022\014\n\004name\030\001 \001(\t\022" +
      "\r\n\005phone\030\002 \001(\t\022\013\n\003zip\030\003 \001(\t\022\017\n\007address\030\004" +
      " \001(\t\022\014\n\004city\030\005 \001(\t\022\027\n\nfarmStatus\030\006 \001(\tH\000" +
      "\210\001\001\022\035\n\020deliveryDistance\030\007 \001(\005H\001\210\001\001B\r\n\013_f" +
      "armStatusB\023\n\021_deliveryDistance\"\025\n\005Offer\022" +
      "\014\n\004name\030\001 \001(\t2)\n\013FarmService\022\032\n\nCreateFa" +
      "rm\022\005.Farm\032\005.FarmB&\n\"mango.sep3.databasea" +
      "ccess.protobufP\001b\006proto3"
    };
    descriptor = com.google.protobuf.Descriptors.FileDescriptor
      .internalBuildGeneratedFileFrom(descriptorData,
        new com.google.protobuf.Descriptors.FileDescriptor[] {
        });
    internal_static_Farm_descriptor =
      getDescriptor().getMessageTypes().get(0);
    internal_static_Farm_fieldAccessorTable = new
      com.google.protobuf.GeneratedMessageV3.FieldAccessorTable(
        internal_static_Farm_descriptor,
        new java.lang.String[] { "Name", "Phone", "Zip", "Address", "City", "FarmStatus", "DeliveryDistance", "FarmStatus", "DeliveryDistance", });
    internal_static_Offer_descriptor =
      getDescriptor().getMessageTypes().get(1);
    internal_static_Offer_fieldAccessorTable = new
      com.google.protobuf.GeneratedMessageV3.FieldAccessorTable(
        internal_static_Offer_descriptor,
        new java.lang.String[] { "Name", });
  }

  // @@protoc_insertion_point(outer_class_scope)
}
