require 'open3'
require 'fileutils'

# [Custom Setting]：You can change value to corresponding your project

class Proto2Csharp

  # main
  def self.Process
    ExecuteCommand(InitSetting(), is_wait_exit: true)
  end

  # Initial
  def self.InitSetting

    # [Custom Setting]：root path
    root_path = File.expand_path('..', Dir.pwd) 

    # [Custom Setting]：proto folder path
    proto_folder_path = File.join(root_path, 'Proto')

    # [Custom Setting]：protobuf compiler path
    protoc_path = if Gem.win_platform?
                    File.join(proto_folder_path, 'protoc.exe')
                  else
                    File.join(proto_folder_path, 'protoc')
                  end

    # [Custom Setting]：proto files' folder path
    proto_target_path = File.join(proto_folder_path, 'ProtoRuby')

    # [Custom Setting]：output files path
    output_path = File.join(root_path, 'RubyMessage')

    # Collect all proto files from "proto_folder_path"
    all_protos = Dir.glob(File.join(proto_target_path, '*.proto'))

    # Return：Construct important setting values to execute cmd
    setting = {
      root_path: root_path,
      proto_folder_path: proto_folder_path,
      protoc_path: protoc_path,
      proto_target_path: proto_target_path,           
      output_path: output_path,
      all_protos: all_protos
    }
  end

  # Excute by Setting
  def self.ExecuteCommand(setting, is_wait_exit: false)  

    #Foreach All Protos and Translate to Csharp
    setting[:all_protos].each do |proto|
      input_filename = File.basename(proto)
      argument = "--ruby_out=#{setting[:output_path]} --proto_path=#{setting[:proto_target_path]} #{proto}"
      cmd = "#{setting[:protoc_path]} #{argument}"
      stdout, stderr, status = Open3.capture3(cmd)

      # Result Message
      input_filename = File.basename(proto)
      if status.success?
        puts "Success to transform #{input_filename} to Ruby file"
      else
        input_filename = File.basename(proto)
        puts "Fail to transform #{input_filename}"
        puts "----------Log----------"
        puts "#{stderr}"
      end
    end
  end
end

#Do
Proto2Csharp.Process