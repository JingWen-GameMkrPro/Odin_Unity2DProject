require 'open3'
require 'fileutils'


class Proto2Csharp
  def self.prepare_setting
    # Custom Setting
    root_path = File.expand_path('..', Dir.pwd) 
    proto_folder_path = File.join(root_path, 'Proto')
    protoc_path = if Gem.win_platform?
                    File.join(proto_folder_path, 'protoc.exe')
                  else
                    File.join(proto_folder_path, 'protoc')
                  end
    protoc_path = if Gem.win_platform?
                    File.join(proto_folder_path, 'protoc.exe')
                  else
                    File.join(proto_folder_path, 'protoc')
                  end
    output_path = File.join(root_path, 'Assets', 'Scripts', 'ProtoMessage')
    all_protos = Dir.glob(File.join(proto_folder_path, '*.proto'))

    setting = {
      root_path: root_path,
      proto_folder_path: proto_folder_path,
      protoc_path: protoc_path,           
      output_path: output_path,
      all_protos: all_protos
    }

    execute(setting, is_wait_exit: true)
  end

  #Excute by Setting
  def self.execute(setting, is_wait_exit: false)  
      is_redirect_standard_output = true
      is_redirect_standard_error = true
      is_use_shell_execute = false

      if Gem.win_platform?
        is_redirect_standard_output = false
        is_redirect_standard_error = false
        is_use_shell_execute = true
      end

      # Adjust values if waiting for the command to exit
      if is_wait_exit
        is_redirect_standard_output = true
        is_redirect_standard_error = true
        is_use_shell_execute = false
      end

      #Foreach all protos
      setting[:all_protos].each do |proto|
        argument = "--csharp_out=#{setting[:output_path]} --proto_path=#{setting[:proto_folder_path]} #{proto}"
        cmd = "#{setting[:protoc_path]} #{argument}"
        is_sucess = system(cmd);
      
      #Message
      if is_sucess
        puts "Success to transform #{proto} to Csharp"
      else
        puts "Fail to transform #{proto} to Csharp"
      end
    end
  end
end

#Do
Proto2Csharp.prepare_setting